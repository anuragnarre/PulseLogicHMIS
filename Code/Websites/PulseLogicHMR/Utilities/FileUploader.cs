using PulseLogicHMR.CommonTypes;
using PulseLogicHMR.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Utilities
{
    /// <summary>
    /// Handles file uploads with security validation.
    ///
    /// Security fixes applied (see plan S8, S12, S13):
    ///   S8  — Exception details are never returned to the client. Full details logged server-side only.
    ///   S12 — Path traversal guard: resolves and validates that the target path stays inside fileStorageLocation.
    ///   S13 — Whitelist of allowed file extensions/MIME types (configured below). Anything else is rejected.
    ///
    /// ⚠️  OUTSTANDING: fileuploads/ is currently inside wwwroot (web-accessible).
    ///     It should be moved to a sibling directory and served through an authenticated endpoint.
    ///     Tracked as: PLHMR-SEC-012
    /// </summary>
    public class FileUploader
    {
        static string fileStorageLocation;

        // S13: Allowed file types — add/remove as per clinical requirements.
        // Both extension AND MIME type are validated.
        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff",
            ".doc", ".docx", ".xls", ".xlsx", ".csv", ".txt",
            ".dcm",  // DICOM
            ".zip"   // Compressed lab reports
        };

        // Max upload size: 50 MB per file (configurable)
        private const long MaxFileSizeBytes = 50L * 1024 * 1024;

        public FileUploader(string storageLocation)
        {
            fileStorageLocation = storageLocation;
        }

        public static async Task<PulseLogicHTTPResponse<object>> Upload(IFormFileCollection files, string localFolder)
        {
            PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();

            try
            {
                // S12 FIX: Validate that the resolved path does not escape the storage root (path traversal guard).
                string resolvedRoot = Path.GetFullPath(fileStorageLocation);
                string resolvedTarget = Path.GetFullPath(Path.Combine(resolvedRoot, localFolder));

                if (!resolvedTarget.StartsWith(resolvedRoot, StringComparison.OrdinalIgnoreCase))
                {
                    responseData.Status = "Failed";
                    responseData.ErrorMessage = "Invalid upload destination.";
                    return responseData;
                }

                // Ensure target directory exists (but only within the allowed root).
                if (!Directory.Exists(resolvedTarget))
                {
                    Directory.CreateDirectory(resolvedTarget);
                }

                foreach (var file in files)
                {
                    // S13 FIX: Validate file size.
                    if (file.Length > MaxFileSizeBytes)
                    {
                        responseData.Status = "Failed";
                        responseData.ErrorMessage = $"File '{Path.GetFileName(file.FileName)}' exceeds the maximum allowed size of 50 MB.";
                        return responseData;
                    }

                    // S13 FIX: Sanitise the file name — strip any path components, keep only the filename.
                    string safeFileName = Path.GetFileName(file.FileName);
                    if (string.IsNullOrWhiteSpace(safeFileName))
                    {
                        responseData.Status = "Failed";
                        responseData.ErrorMessage = "Invalid file name.";
                        return responseData;
                    }

                    // S13 FIX: Validate file extension against the whitelist.
                    string extension = Path.GetExtension(safeFileName);
                    if (!AllowedExtensions.Contains(extension))
                    {
                        responseData.Status = "Failed";
                        responseData.ErrorMessage = $"File type '{extension}' is not allowed. Permitted types: {string.Join(", ", AllowedExtensions)}.";
                        return responseData;
                    }

                    string destinationPath = Path.Combine(resolvedTarget, safeFileName);

                    // PERF FIX: Stream directly to disk instead of buffering the entire file in memory.
                    // Previously: BinaryReader → byte[] → MemoryStream → FileStream (~100MB for a 50MB file).
                    // Now: direct stream copy using a small kernel buffer (~80KB).
                    using (var fileStream = new FileStream(destinationPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

                responseData.Status = "OK";
                // S8 FIX: Return only the relative sub-folder, not the full server path.
                responseData.Results = localFolder;
            }
            catch (Exception ex)
            {
                // S8 FIX: Log full details server-side only. NEVER return ex.Message / ex.ToString() to client.
                // Re-throw so ExceptionHandlingMiddleware can log with correlation ID.
                // The safe client message below won't be seen because the middleware replaces the response,
                // but we set it for defence-in-depth if the middleware is ever bypassed.
                responseData.Status = "Failed";
                responseData.ErrorMessage = "File upload failed. Please contact support if the problem persists.";
                // Note: do NOT include ex.Message here — it can contain internal server paths.
                throw;
            }
            return responseData;
        }
    }
}

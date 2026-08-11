# PulseLogic HMR v2.0.1 - Feature and Patch Notes

## 🚀 Key Highlights & New Features

### 🔌 GitHub Model Context Protocol (MCP) Server Integration
Added support and setup instructions for the **GitHub MCP Server**. This enables AI coding agents (such as Antigravity, Cursor, and Claude Desktop) to interact directly with the `PulseLogicHMIS` GitHub repository using a GitHub Personal Access Token (PAT).

* **Direct GitHub Operations**: Search repositories, manage issues, pull requests, commits, branches, and code reviews directly via MCP.
* **Configuration Guide**: Included step-by-step setup guides for VS Code / Antigravity IDE, Cursor, and Claude Desktop.

---

## 🛠️ Security, Performance & Stability Patches

### 🛡️ Security Hardening
* **JWT Production Startup Guard**: Updated `ConfigureServices.cs` to throw a fatal `InvalidOperationException` on startup in non-development environments if the `ASPNETCORE_JWT_KEY` environment variable is not set. Prevents starting the server in production with insecure fallback keys.
* **JWT Token Key Alignment**: Aligned `GenerateJwtToken()` in `AccountController.cs` to check the `ASPNETCORE_JWT_KEY` environment variable first, matching `ConfigureServices.cs` and `PulseLogicDataFilter.cs`. Resolves signature mismatches that previously caused silent 401 Unauthorized errors in production.
* **Global API Rate Limiting**: Applied `[EnableRateLimiting("api-general")]` to the `CommonController` base class, protecting all derived API endpoints with a 300 requests/minute sliding window rate limiter.

### ⚡ Performance & Resource Management
* **File Upload Memory Streaming**: Refactored `FileUploader.Upload()` to stream directly to disk using `file.CopyToAsync(fileStream)` with an 80KB kernel buffer. Eliminates ~100MB RAM allocations per 50MB file upload (previously double-buffered in memory via `BinaryReader` & `MemoryStream`).
* **DbContext Connection Leak Fixes**: Wrapped manual `DbContext` instances (`CoreDbContext`, `SystemAdminDbContext`, `RbacDbContext`, `MasterDbContext`) in `using` blocks across `AccountController.cs`, `CommonController.cs`, and `PulseLogicRBAC.cs` to guarantee connection disposal and prevent pool exhaustion.

### 🐛 Bug Fixes & Reliability
* **Null Safety in Parameter Utility**: Fixed 5 unsafe `data[keyName].Value<string>()` calls in `CommonFunctions.cs` by introducing null-conditional `?.` access. Prevents `NullReferenceException` crashes when JSON parameter keys are missing.
* **Exception Re-throw in Uploads**: Added `throw;` to `FileUploader.Upload()` so I/O failures bubble up to `ExceptionHandlingMiddleware` for server-side logging with correlation IDs.
* **Diagnostic Read Logging**: Added error logging to `ReadBodyAsString()` in `PulseLogicActionFilter.cs` to capture stream read errors prior to JSON parsing.
* **Code Cleanup**: Added descriptive message to `[Obsolete("Use GetCoreParameterValueByKeyName_Boolean instead.")]` in `CommonFunctions.cs` and removed dead/unfinished method `GetCoreParameterValueByKeyName_IntArray`.

---

## 📐 Architecture Documentation Suite
Added a comprehensive suite of architecture documentation in the `architecture/` directory detailing the system's N-Tier design:

* **System Overview (`01_System_Overview.md`)**: High-level interaction maps between the Angular SPA, ASP.NET Core Web API, and SQL databases.
* **Frontend Architecture (`02_Frontend_Architecture.md`)**: Breakdown of the Angular SPA, including lazy-loaded modules (Clinical, Billing, Pharmacy) and component structures.
* **Backend API Architecture (`03_Backend_API_Architecture.md`)**: Explanation of the ASP.NET Core request pipeline, modular Controllers, and Service layers.
* **Data Access & Core (`04_Data_Access_and_Core.md`)**: Details on `DalLayer`, `ServerModel`, and EF Core repositories.
* **Security & Cross-Cutting Concerns (`05_Security_and_CrossCutting.md`)**: Information on custom RBAC, JWT integrations, SQL auditing, and `ExceptionHandlingMiddleware`.

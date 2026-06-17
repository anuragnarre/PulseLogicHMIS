using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PulseLogicHMR.Utilities;

namespace PulseLogicHMR.Utilities
{
    /// <summary>
    /// Global exception handling middleware.
    ///
    /// Responsibilities:
    ///   1. Catch ALL unhandled exceptions that bubble up from the MVC pipeline.
    ///   2. Log the full exception details internally (with correlation ID).
    ///   3. Return a structured JSON error response to the client — WITHOUT leaking
    ///      stack traces or internal exception details (SECURITY FIX).
    ///
    /// Design: registered before UseMvc() in Startup.Configure() so it wraps the full pipeline.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next   = next   ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException knfEx)
            {
                // 404-class errors (entity not found): safe to surface the message.
                await HandleExceptionAsync(context, knfEx, HttpStatusCode.NotFound, knfEx.Message);
            }
            catch (ArgumentException argEx)
            {
                // 400-class errors (bad input): safe to surface the message.
                await HandleExceptionAsync(context, argEx, HttpStatusCode.BadRequest, argEx.Message);
            }
            catch (InvalidOperationException invEx)
            {
                // 422-class business rule violations: surface message only.
                await HandleExceptionAsync(context, invEx, (HttpStatusCode)422, invEx.Message);
            }
            catch (Exception ex)
            {
                // 500 — SECURITY FIX: log full details internally, NEVER send stack trace to client.
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError,
                    "An unexpected server error occurred. Please contact support.");
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception ex,
            HttpStatusCode statusCode,
            string clientMessage)
        {
            // Generate a unique correlation ID so the error can be traced in logs.
            string correlationId = context.TraceIdentifier ?? Guid.NewGuid().ToString();

            // Log the FULL exception details — including stack trace — only in the server log.
            _logger.LogError(ex,
                "Unhandled exception [{CorrelationId}] {Method} {Path} — {ExceptionType}: {Message}",
                correlationId,
                context.Request.Method,
                context.Request.Path,
                ex.GetType().Name,
                ex.Message);

            // Build a safe, structured JSON response (no stack trace, no internal paths).
            context.Response.ContentType = "application/json";
            context.Response.StatusCode  = (int)statusCode;

            var errorResponse = new
            {
                Status       = "Failed",
                ErrorMessage = clientMessage,
                CorrelationId = correlationId     // lets the client quote this ID when contacting support
            };

            await context.Response.WriteAsync(
                PulseLogicJSONConvert.SerializeObject(errorResponse));
        }
    }
}

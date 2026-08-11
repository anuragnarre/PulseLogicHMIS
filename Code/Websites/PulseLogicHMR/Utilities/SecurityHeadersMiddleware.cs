using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PulseLogicHMR.Utilities
{
    /// <summary>
    /// S17 SECURITY FIX: Security headers middleware.
    ///
    /// Injects HTTP security headers on every response:
    ///   • X-Frame-Options: DENY          — Prevents this app being embedded in iframes (clickjacking).
    ///   • X-Content-Type-Options: nosniff — Prevents browsers from MIME-sniffing responses.
    ///   • X-XSS-Protection: 1; mode=block — Legacy XSS filter (for older browsers).
    ///   • Referrer-Policy: no-referrer   — Prevents the Referrer header leaking internal URLs.
    ///   • Permissions-Policy            — Disables dangerous browser features not needed by the app.
    ///   • Content-Security-Policy       — SEC-09 FIX: Restricts script/style/resource origins.
    ///
    /// Registered in Startup.Configure BEFORE UseSession/UseMvc so it applies to all responses.
    /// </summary>
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        // SEC-09: Baseline CSP for an Angular 7 / ASP.NET Core app.
        //   • script-src 'self'           — Only scripts from our own origin; no inline scripts.
        //   • style-src 'self' 'unsafe-inline' — Angular Material + chart libs use inline styles.
        //   • img-src 'self' data:         — data: URIs used for inline avatars and chart images.
        //   • font-src 'self'              — Local fonts only.
        //   • connect-src 'self'           — XHR/fetch only back to our own API.
        //   • frame-ancestors 'none'       — Reinforces X-Frame-Options: DENY.
        //   • object-src 'none'            — No Flash/plugin content.
        //   • base-uri 'self'              — Prevents base tag hijacking.
        private const string ContentSecurityPolicy =
            "default-src 'self'; " +
            "script-src 'self'; " +
            "style-src 'self' 'unsafe-inline'; " +
            "img-src 'self' data:; " +
            "font-src 'self'; " +
            "connect-src 'self'; " +
            "frame-ancestors 'none'; " +
            "object-src 'none'; " +
            "base-uri 'self';";

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Prevent the application being framed (clickjacking protection).
            context.Response.Headers["X-Frame-Options"] = "DENY";

            // Prevent MIME-type sniffing (browser must honour declared Content-Type).
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";

            // Legacy XSS filter for browsers that still support it (IE/Edge legacy).
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

            // Do not send Referrer header when navigating away from the app.
            context.Response.Headers["Referrer-Policy"] = "no-referrer";

            // Disable access to sensitive browser APIs not required by this application.
            context.Response.Headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=()";

            // SEC-09 FIX: Content-Security-Policy — blocks inline script injection (XSS),
            // restricts resource loading to same origin. See constant above for details.
            context.Response.Headers["Content-Security-Policy"] = ContentSecurityPolicy;

            // Cache-Control: do not cache API responses that may contain PHI.
            // Static assets (.js, .css, .png, etc.) override this via StaticFiles middleware.
            if (!context.Request.Path.StartsWithSegments("/PulseLogicApp/dist") &&
                !context.Request.Path.StartsWithSegments("/themes") &&
                !context.Request.Path.StartsWithSegments("/assets"))
            {
                context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
            }

            await _next(context);
        }
    }
}

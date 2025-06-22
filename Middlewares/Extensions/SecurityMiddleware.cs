using System.Security.Cryptography.X509Certificates;

namespace Middlewares.Extensions
{
    public static class SecurityMiddleware
    {
        public static IApplicationBuilder UseSecureHeaders(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                // X-Content-Type-Options
                // Prevents browsers from MIME-sniffing a response away from the declared content-type.
                context.Response.Headers["X-Content-Type-Options"] = "nosniff";

                // X-Frame-Options
                // Prevents the page from being displayed in a frame, protecting against clickjacking attacks.
                context.Response.Headers["X-Frame-Options"] = "DENY";

                // X-XSS-Protection
                // Enables the cross-site scripting (XSS) filter in browsers.
                context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

                // Referrer-Policy
                // Controls the amount of referrer information that is passed when navigating from your site.
                context.Response.Headers["Referrer-Policy"] = "no-referrer";

                // Content-Security-Policy
                // Helps prevent a variety of attacks such as cross-site scripting (XSS) and data injection attacks.
                context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self'; style-src 'self'; img-src 'self' ; font-src 'self' ; connect-src 'self'";

                await next();
            });
            return app;
        }

        public static IApplicationBuilder UseStrictTransportSecurity(this IApplicationBuilder app, int maxAgeInSeconds = 31536000, bool includeSubDomains = true, bool preload = false)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers["Strict-Transport-Security"] = $"max-age={maxAgeInSeconds}; {(includeSubDomains ? "includeSubDomains; " : "")}{(preload ? "preload" : "")}";
                await next();

            });
            return app;
        }

        
        public static IApplicationBuilder UserPermissionPolicy(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                // Permissions Policy
                // Controls which features and APIs can be used in the browser.
                context.Response.Headers["Permissions-Policy"] = "geolocation=(self), microphone=(), camera=(), fullscreen=(), payment=()";
                await next();
            });
            return app;
        }

        public static IApplicationBuilder UseCrossOriginPolicy(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                // Access-Control-Allow-Origin
                // Specifies which origins are allowed to access the resources.
                context.Response.Headers["Access-Control-Allow-Origin"] = "https://trusted.example.com";

                // Access-Control-Allow-Methods
                // Specifies which HTTP methods are allowed when accessing the resource.
                context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE";

                // Access-Control-Allow-Headers
                // Specifies which headers can be used when making the actual request.
                context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
                await next();
            });
            return app;
        }


        // Especially for e-commerce applications, it is important to limit the size of requests to prevent denial of service attacks.
        /// <summary>
        /// UseRequestSizelimit
        /// </summary>
        /// <param name="app"></param>
        /// <param name="maxBytes">Default: 1MB</param>
        /// <returns></returns>
        public static IApplicationBuilder UseRequestSizelimit(this IApplicationBuilder app, long maxBytes = 1024 * 1024 )
        {
            app.Use(async (context, next) =>
            {
                // Request Size Limit
                // Limits the size of the request body to prevent denial of service attacks.
                if (context.Request.ContentLength > maxBytes)
                {
                    context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
                    context.Response.ContentType = "application/json";

                    var errorMessage = new
                    {
                        error = "Request Entity Too Large",
                        maxSize = maxBytes,
                        status = 413
                    };

                    var json = System.Text.Json.JsonSerializer.Serialize(errorMessage);
                    await context.Response.WriteAsync(json);
                    return;
                }
                // If the request size is within the limit, continue processing the request.
                await next();
            });
            return app;
        }

    }
}

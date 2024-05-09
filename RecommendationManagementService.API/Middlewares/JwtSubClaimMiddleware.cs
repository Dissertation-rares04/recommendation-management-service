using System.Security.Claims;

namespace RecommendationManagementService.API.Middlewares
{
    public class JwtSubClaimMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtSubClaimMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Extract sub claim from JWT token in request headers
                var subClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (subClaim == null)
                {
                    throw new Exception("No sub claim found");
                }
   
                // Add sub claim to request context
                context.Items["UserId"] = subClaim;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // You may want to log the exception for troubleshooting
                await HandleException(context, ex);
            }

            // Call the next middleware in the pipeline
            await _next(context);
        }

        private async Task HandleException(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                message = ex.Message
            });
        }
    }
}

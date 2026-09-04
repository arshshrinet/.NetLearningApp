using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LearningApp.Exceptions
{
    internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception occurred.");

            var isDevelopment = httpContext.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment();

            httpContext.Response.ContentType = "application/problem+json";

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().FullName,
                    Title = isDevelopment ? exception.Message : "An unexpected error occurred.",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = isDevelopment ? exception.Message : null
                }
            });
        }
    }
}

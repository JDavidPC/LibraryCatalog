using Catalog.Application.Exceptions;
using Catalog.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Handlers
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                                    Exception exception,
                                                    CancellationToken cancellationToken)
        {
            (int status, string title, string detail) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Recurso no encontrado", exception.Message),
                BusinessRuleException => (StatusCodes.Status400BadRequest, "Regla de negocio incumplida", exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "Error interno del servidor", "Ocurrió un error inesperado.")
            };

            if (status == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, "Error no controlado al procesar la solicitud.");
            }

            ProblemDetails problem = new()
            {
                Status = status,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = status;

            await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

            return true;
        }
    }
}

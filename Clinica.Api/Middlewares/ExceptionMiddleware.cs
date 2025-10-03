using System.Net;
using Clinica.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Clinica.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _env = env;
        _logger = logger;
    }

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            var (status, title, type) = Map(ex);
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,                     // mensagem “bonita” pro cliente
                Type = type,                       // link semântico (opcional)
                Detail = _env.IsDevelopment() ? ex.Message : null, // sem stack em prod
                Instance = ctx.Request.Path
            };

            problem.Extensions["traceId"] = ctx.TraceIdentifier;

            ctx.Response.ContentType = "application/problem+json";
            ctx.Response.StatusCode = status;


            await ctx.Response.WriteAsJsonAsync(problem);
        }
    }

    private static (int status, string title, string type) Map(Exception ex) => ex switch
    {
        CannotUnloadAppDomainException => (StatusCodes.Status422UnprocessableEntity, ex.Message, "https://httpstatuses.io/422"),
        InvalidOperationException => (StatusCodes.Status409Conflict, ex.Message, "https://httpstatuses.io/409"),
        KeyNotFoundException => (StatusCodes.Status404NotFound, ex.Message, "https://httpstatuses.io/404"),
        DbUpdateException => (StatusCodes.Status409Conflict, "Violação de integridade no banco.", "https://httpstatuses.io/409"),
        _ => (StatusCodes.Status500InternalServerError, "Erro interno do servidor.", "https://httpstatuses.io/500")
    };

}

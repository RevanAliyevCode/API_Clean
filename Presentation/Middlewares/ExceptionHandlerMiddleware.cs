using System;
using API.Domain.Exceptions;
using Business.Wrappers;

namespace API.Presentation.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var response = new ResponseError();

            switch (ex)
            {
                case ValidationException e:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Errors = e.Errors;
                    break;
                case NotFoundException e:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.Errors = e.Errors;
                    break;
                default:
                    _logger.LogError($"Message: {ex.Message} StackTrace: {ex.StackTrace} InnerException: {ex.InnerException}");
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Errors.Add("An error occured");
                    break;
            }

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

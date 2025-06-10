using System;
using CustomerSupport.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace CustomerSupport.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var errorResponse = new ErrorResponse();

        _logger.LogInformation($"{exception.Message}");
        errorResponse.Message = exception.Message;

        switch (exception)
        {
            case EntityEmptyException:
                errorResponse.StatusCode = 404;
                httpContext.Response.StatusCode = 404;
                break;
            case ItemNotFoundException:
                errorResponse.StatusCode = 404;
                httpContext.Response.StatusCode = 404;
                break;
            case PassowrdWrongException:
                errorResponse.StatusCode = 401;
                httpContext.Response.StatusCode = 401;
                break;
            case InvalidTokenException:
                errorResponse.StatusCode = 401;
                httpContext.Response.StatusCode = 401;
                break;
            case UnsupportedFileUploadException:
                errorResponse.StatusCode = 405;
                httpContext.Response.StatusCode = 405;
                break;
            case UnauthorizedAccessException:
                errorResponse.StatusCode = 401;
                httpContext.Response.StatusCode = 401;
                break;

            default:
                errorResponse.StatusCode = 500;
                httpContext.Response.StatusCode = 500;
                break;
        }

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
        return true;
    }
}

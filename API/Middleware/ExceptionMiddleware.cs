using System;
using System.Text.Json;

using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(IHostEnvironment environment, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, environment);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment environment)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

        var apiResponse = environment.IsDevelopment()
            ? new ApiErrorResponse(response.StatusCode, ex.Message, ex.StackTrace?.ToString())
            : new ApiErrorResponse(response.StatusCode, "Internal Server Error");

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var json = JsonSerializer.Serialize(apiResponse, options);

        await context.Response.WriteAsync(json);
    }
}

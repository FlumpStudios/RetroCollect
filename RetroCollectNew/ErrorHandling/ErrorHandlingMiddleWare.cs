using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Data;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context /* other scoped dependencies */)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; // 500 if unexpected
        string logText = exception.GetType().ToString() + " exception thown, returned Internal Server error with following exception: " + exception;
        if (exception is DbUpdateConcurrencyException)
        {
            code = HttpStatusCode.NotFound;
            logText = "Not found exception thrown, returned Not Found with following exception: " + exception;
        }
        else if (exception is UnauthorizedAccessException)
        {
            code = HttpStatusCode.Unauthorized;
            logText = "Unauthorized exception thrown, returned Unauthorized with following exception" + exception;
        }
        else if (exception is DataException)
        {
            code = HttpStatusCode.BadRequest;
            logText = "DataException exception thrown, returned Bad Request with following exception: " + exception;
        }
        else if (exception is DbUpdateConcurrencyException)
        {
            logText = "DbUpdateConcurrencyException exception thrown, returned Not Found with following exception: " + exception;
            code = HttpStatusCode.NotFound;
        }

        //TODO: Add logging here
        //Logging.Log().Error(logText);


        var result = JsonConvert.SerializeObject(new { error = exception.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }
}
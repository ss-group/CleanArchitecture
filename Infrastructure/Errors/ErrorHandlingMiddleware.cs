using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Errors
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(
            ILogger<ErrorHandlingMiddleware> logger,
           RequestDelegate next)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error middleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            object errors = null;

            switch (exception)
            {
                case RestException re:
                    errors = re.Errors;
                    context.Response.StatusCode = (int)re.Code;
                    break;
                case DbUpdateConcurrencyException ce:
                    errors = new { Code = "message.STD00001" };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case DbUpdateException ue:
                    if(ue.InnerException.GetType() == typeof(PostgresException))
                    {
                        PostgresException innerException = ue.InnerException as PostgresException;
                        errors = new { Code = GetExceptionMessage(innerException) };
                    }
                    else
                    {
                        errors = new { Code = ue.InnerException.Message };
                    }
                  
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case Exception e:
                    errors = new { Code = GetExceptionMessage(e) };
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";

            var result = JsonConvert.SerializeObject(new
            {
                errors
            }, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

            await context.Response.WriteAsync(result);
        }

        private static string GetExceptionMessage(PostgresException exception)
        {
            if (exception.SqlState == "23503")
            {
                var isDelete = exception.MessageText.Contains("delete");
                return isDelete ? "message.SQL23503D" : "message.SQL23503A";
            }
            else return $"message.SQL{exception.SqlState}";

        }

        private static string GetExceptionMessage(Exception exception)
        {
            if (exception.Message.Contains("Value cannot be null.\r\nParameter name: entity") && ((System.ArgumentException)exception).ParamName == "entity")
            {
                return "message.STD00001";
            }
            else if (string.IsNullOrEmpty(exception.Message))
            {
                return "Error";
            }
            else
            {
                return exception.Message;
            }
        }
    }
}

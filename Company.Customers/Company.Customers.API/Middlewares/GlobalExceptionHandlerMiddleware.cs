using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;

namespace Company.Customers.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private static readonly int _codigoErroInterno = (int)HttpStatusCode.InternalServerError;
        private const string CORRELATION_ID = "x-correlation-id";
        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                string correlationId = context.Request.Headers[CORRELATION_ID];

                if (string.IsNullOrEmpty(correlationId))
                    correlationId = Guid.NewGuid().ToString();

                context.Response.Headers.Add(CORRELATION_ID, correlationId);
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = _codigoErroInterno;
            var correlationId = context.Response.Headers[CORRELATION_ID].ToString();

            var expando = new ExpandoObject();
            expando.TryAdd(CORRELATION_ID, correlationId);
            expando.TryAdd("status-code", context.Response.StatusCode);
            expando.TryAdd("details", exception.Message);

            string json = JsonConvert.SerializeObject(expando);
            _logger.LogError(json);
            return context.Response.WriteAsync(json);
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace API.Middlwares
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IWebHostEnvironment env, RequestDelegate next)
        {
            _logger = logger;
            _env = env;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, exc.Message);
                int statusCode = (int)HttpStatusCode.InternalServerError;
                string message = exc.Message ?? "Internal Server Error";

                var httpExceptionObject = new HttpException
                {
                    StatusCode = statusCode,
                    Message = message,
                    Error = _env.IsDevelopment() ? exc.ToString() : ""
                };

                context.Response.ContentType = MediaTypeNames.Application.Json;
                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsJsonAsync(httpExceptionObject);
            }
        }

        class HttpException
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string Error { get; set; }
        }

    }
}

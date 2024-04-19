﻿using System.Net;
using System.Text.Json;
using ISc.Shared;
using ISc.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace ISc.Presentation.Middlerware
{
    public class GlobalErrorHandler
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {

            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                var response = new Shared.Response()
                {
                    Data = e.GetType().Name,
                    Message = e.Message,
                    StatusCode = ExceptionStatusCode(e)
                };

                var json = JsonSerializer.Serialize(response);
                context.Response.ContentType = "application/json";
                context?.Response.WriteAsync(json);

            }
        }
        private HttpStatusCode ExceptionStatusCode(Exception ex)
        {
            var exceptionType = ex.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                return HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(Shared.Exceptions.KeyNotFoundException))
            {
                return HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(SerivceErrorException))
            {
                return HttpStatusCode.BadGateway;
            }
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}

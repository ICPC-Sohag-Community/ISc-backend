using ISc.Shared;
using ISc.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
                if (context.Response.StatusCode == 401)
                    throw new UnAuthorized("UnAuthorized");
            }
            catch (Exception e)
            {

                var json = JsonSerializer.Serialize(await Response.FailureAsync(e.Message));
                context.Response.ContentType = "application/json";
                context?.Response.WriteAsync(json);

            }
        }   
    }
}

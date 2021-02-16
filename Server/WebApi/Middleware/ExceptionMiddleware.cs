
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                //switch (error)
                //{
                //    case AppException e:
                //        // custom application error
                //        response.StatusCode = (int)HttpStatusCode.BadRequest;
                //        break;
                //    case KeyNotFoundException e:
                //        // not found error
                //        response.StatusCode = (int)HttpStatusCode.NotFound;
                //        break;
                //    default:
                //        // unhandled error
                //        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //        break;
                //}

                //var result = JsonSerializer.Serialize(new { message = error?.Message });

                response.StatusCode = (int)HttpStatusCode.BadRequest;
                //await response.WriteAsync(result);
            }
        }
    }
}

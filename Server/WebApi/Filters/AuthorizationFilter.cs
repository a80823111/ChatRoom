using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Model.BaseModels;
using Model.Enum.ApiResponseType;
using Newtonsoft.Json;
using Service.Extensions;
using Service.Interfaces.IUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Filters
{
    public class AuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //判斷控制器是不是有AllowAnonymousAttribute
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                var actionAttributes = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true);

                if (actionAttributes.Any(x => x.GetType() == typeof(AllowAnonymousAttribute)))
                    return;
            }

            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                //App認證授權     
                context = await AppAuthorizationAsync(context);
            }

            await context.HttpContext.Response.CompleteAsync();
        }

        /// <summary>
        /// App認證授權
        /// </summary>
        /// <param name="context"></param>
        public async Task<AuthorizationFilterContext> AppAuthorizationAsync(AuthorizationFilterContext context)
        {
            var _authService = context.HttpContext.RequestServices.GetService(typeof(IAuthService)) as IAuthService;

            if (await _authService.AuthorizationAsync(context.HttpContext))
            {
                return context;
            }
            else
            {
                return Unauthorized(context);
            }

        }

        /// <summary>
        /// 授權失敗
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private AuthorizationFilterContext Unauthorized(AuthorizationFilterContext context)
        {
            //Api授權失敗
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(
                new ApiRes { Status = ApiResType.Failed }
                ));
            return context;
        }

    }
}

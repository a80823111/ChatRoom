using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.BaseModels;
using Model.Enum.ApiResponseType;
using Model.ResponseModels;
using Model.ViewModels;
using Service.Interfaces.IUsers;

namespace WebApi.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IAuthService _authService;

        public UsersController(ILoginService loginService, IAuthService authService)
        {

            _loginService = loginService;
            _authService = authService;

        }
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost, Route("api/Users/Login")]
        public async Task<ApiRes> AppLogin(LoginViewModel loginViewModel)
        {
            var apiResponse = new ApiRes();

            var users = await _loginService.GetUsersByNickNameAsync(loginViewModel.NickName);

            if (users == null)
            {
                users = await _loginService.InsertUsersAsync(loginViewModel.NickName);

            }


            apiResponse.Status = ApiResType.Success;
            apiResponse.Result = new LoginResModel { UsersId = users.Id,Token = _authService.GenerateToken(users) };

            return apiResponse;
        }


    }
}
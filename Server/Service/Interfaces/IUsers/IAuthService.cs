using Microsoft.AspNetCore.Http;
using Model.Migrations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.IUsers
{
    public interface IAuthService
    {
        /// <summary>
        /// 驗證授權
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        Task<bool> AuthorizationAsync(HttpContext httpContext,string authorizationToken = null);

        /// <summary>
        /// 產生JwtToken
        /// </summary>
        string GenerateToken(Users users);

        /// <summary>
        /// 解密Jwt
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal DecryptJwt(string token);

        /// <summary>
        /// 建立登入識別
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<Claim> CreateClaims(Users user);

        /// <summary>
        /// 取得Jwt中的UserId
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        string GetLoginUsersId(ClaimsPrincipal claimsPrincipal);

        /// <summary>
        /// 取得登入UserId
        /// </summary>
        string LoginUsersId(HttpContext httpContext);

        /// <summary>
        /// 用戶端Ip位置 , 使用需注入BaseInfoExtensions
        /// </summary>
        string ClientIpAddress(HttpContext httpContext);

        /// <summary>
        /// 取得正確的登入資訊
        /// </summary>
        /// <returns></returns>
        ClaimsPrincipal GetCurrentClaimsPrincipal(HttpContext httpContext);

        /// <summary>
        /// 取得AuthorizationToken
        /// </summary>
        /// <returns></returns>
        string GetAuthorizationToken(HttpContext httpContext);



    }
}

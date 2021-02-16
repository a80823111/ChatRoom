using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Model.BaseModels.Configuration;
using Model.Migrations;
using Repository.Repositories;
using Service.Interfaces.IUsers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UsersService : ILoginService, IAuthService, IUsersService
    {
        private readonly UsersRepository _userRepository;

        public UsersService(UsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 取得Users
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        public async Task<Users> GetUsersByIdAsync(string usersId)
        {
            return await _userRepository.GetUserAsync(usersId);
        }

        /// <summary>
        /// 取得Users 暱稱
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        public async Task<Users> GetUsersByNickNameAsync(string nickName)
        {
            return await _userRepository.GetUserByNickNameAsync(nickName);
        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        public async Task<Users> InsertUsersAsync(string nickName)
        {
            var users = new Users
            {
                NickName = nickName,
                CreateTime = DateTime.Now
            };

            await _userRepository.InsertUserAsync(users);

            return users;
        }

        #region Auth
        /// <summary>
        /// 驗證授權
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task<bool> AuthorizationAsync(HttpContext httpContext,string authorizationToken = null)
        {
            if(String.IsNullOrEmpty(authorizationToken))
            {
                authorizationToken = httpContext.Request.Headers["Authorization"];
            }

            //取不到AuthorizationToken
            if (String.IsNullOrEmpty(authorizationToken))
            {
                return false;
            }

            //Jwt解密
            var jwt = DecryptJwt(authorizationToken);

            //JwtToken解密失敗 , 失敗都是無效Token
            if (jwt == null)
            {
                return false;
            }

            var userId = GetLoginUsersId(jwt);

            //Jwt內部不含帳號及使用者類型錯誤 , 理論上不會發生
            if (String.IsNullOrEmpty(userId))
            {
                return false;
            }

            var users = await GetUsersByIdAsync(userId);

            //檢查帳號是否有效
            if (users == null)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region JWT
        /// <summary>
        /// 產生JwtToken
        /// </summary>
        public string GenerateToken(Users users)
        {
            // 設定要加入到 JWT Token 中的聲明資訊(Claims)
            var claims = CreateClaims(users);

            var userClaimsIdentity = new ClaimsIdentity(claims);

            // 建立一組對稱式加密的金鑰，主要用於 JWT 簽章之用
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(LoginSettings.Jwt.SignKey));

            // HmacSha256 有要求必須要大於 128 bits，所以 key 不能太短，至少要 16 字元以上
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // 建立 SecurityTokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = LoginSettings.Jwt.Issuer,
                Audience = LoginSettings.Jwt.Audience,
                Subject = userClaimsIdentity,
                Expires = DateTime.Now.AddMinutes(LoginSettings.Jwt.Expiration),
                SigningCredentials = signingCredentials,

            };

            // 產出所需要的 JWT securityToken 物件，並取得序列化後的 Token 結果(字串格式)
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }

        /// <summary>
        /// 解密Jwt
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ClaimsPrincipal DecryptJwt(string token)
        {
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

                // 建立一組對稱式加密的金鑰，主要用於 JWT 簽章之用
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(LoginSettings.Jwt.SignKey));

                // HmacSha256 有要求必須要大於 128 bits，所以 key 不能太短，至少要 16 字元以上
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                //建立JWT驗證方式
                TokenValidationParameters validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireSignedTokens = true,
                    ValidateLifetime = true,
                    ValidIssuer = LoginSettings.Jwt.Issuer,
                    ValidAudience = LoginSettings.Jwt.Audience,
                    IssuerSigningKeys = new List<SecurityKey> { new SymmetricSecurityKey(Encoding.UTF8.GetBytes(LoginSettings.Jwt.SignKey)) }

                };


                SecurityToken validatedToken;

                return handler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 建立登入識別
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public List<Claim> CreateClaims(Users user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            return claims;
        }


        /// <summary>
        /// 取得Jwt中的UserId
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public string GetLoginUsersId(ClaimsPrincipal claimsPrincipal)
        {
            var usersId = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (usersId != null)
            {
                return usersId.Value;
            }
            else
            {
                return null;
            }
        }

        #endregion


        #region SetBaseExtensions Method
        /// <summary>
        /// 取得登入UserId
        /// </summary>
        public string LoginUsersId(HttpContext httpContext)
        {
            var claimsPrincipal = GetCurrentClaimsPrincipal(httpContext);

            if (claimsPrincipal == null)
            {
                return null;
            }

            return GetLoginUsersId(claimsPrincipal);
        }

        /// <summary>
        /// 用戶端Ip位置 , 使用需注入BaseInfoExtensions
        /// </summary>
        public string ClientIpAddress(HttpContext httpContext)
        {
            return httpContext?.Connection?.RemoteIpAddress?.ToString();
        }

        /// <summary>
        /// 取得正確的登入資訊
        /// </summary>
        /// <returns></returns>
        public ClaimsPrincipal GetCurrentClaimsPrincipal(HttpContext httpContext)
        {
            ClaimsPrincipal claimsPrincipal = null;

            //取得Header.Authorization
            StringValues token = httpContext.Request.Headers["Authorization"];

            if (token.Count > 0)
            {
                claimsPrincipal = DecryptJwt(token);
            }
            else if (httpContext.User.Identity.IsAuthenticated)
            {
                claimsPrincipal = httpContext.User;

            }
            return claimsPrincipal;
        }

        /// <summary>
        /// 取得AuthorizationToken
        /// </summary>
        /// <returns></returns>
        public string GetAuthorizationToken(HttpContext httpContext)
        {
            StringValues token = httpContext.Request.Headers["Authorization"];
            if (token.Count > 0)
            {
                return token.FirstOrDefault();
            }
            else
            {
                return null;
            }

        }
        #endregion
    }
}

using Model.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.IUsers
{
    public interface ILoginService
    {
        /// <summary>
        /// 取得Users
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        Task<Users> GetUsersByIdAsync(string usersId);

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        Task<Users> InsertUsersAsync(string nickName);

        /// <summary>
        /// 取得Users 暱稱
        /// </summary>
        /// <param name="nickName"></param>
        /// <returns></returns>
        Task<Users> GetUsersByNickNameAsync(string nickName);
    }
}

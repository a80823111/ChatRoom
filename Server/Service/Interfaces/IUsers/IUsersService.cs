using Model.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.IUsers
{
    public interface IUsersService
    {
        /// <summary>
        /// 取得Users
        /// </summary>
        /// <param name="usersId"></param>
        /// <returns></returns>
        Task<Users> GetUsersByIdAsync(string usersId);
    }
}

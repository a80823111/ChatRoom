using Model.Enum.SocketConnectType;
using Model.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.ISocket
{
    public interface ISocketService
    {
        /// <summary>
        /// 更新Socket連線資訊
        /// </summary>
        /// <param name="usersId"></param>
        /// <param name="connectId"></param>
        /// <param name="socketConnectOnlineType"></param>
        /// <returns></returns>
        Task<bool> UpsertSocketConnectAsync(string usersId, string connectId, SocketConnectOnlineType socketConnectOnlineType);

        /// <summary>
        /// 取得使用者連線資訊
        /// </summary>
        /// <param name="usersIds"></param>
        /// <returns></returns>
        Task<List<SocketConnect>> GetListSocketConnectByUsersIdsAsync(List<string> usersIds);

        /// <summary>
        /// 取得使用者 By ConnectId
        /// </summary>
        /// <param name="connectId"></param>
        /// <returns></returns>
        Task<SocketConnect> GetListSocketConnectByConnectIdAsync(string connectId);
    }
}

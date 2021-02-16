using Microsoft.AspNetCore.SignalR;
using Model.Enum.SocketConnectType;
using Model.Migrations;
using Repository.Repositories;
using Service.Interfaces.ISocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SocketService:  ISocketService
    {
        private readonly SocketConnectRepository _socketConnectRepository;

        public SocketService(SocketConnectRepository socketConnectRepository)
        {
            _socketConnectRepository = socketConnectRepository;
        }

        /// <summary>
        /// 更新Socket連線資訊
        /// </summary>
        /// <param name="usersId"></param>
        /// <param name="connectId"></param>
        /// <param name="socketConnectOnlineType"></param>
        /// <returns></returns>
        public async Task<bool> UpsertSocketConnectAsync(string usersId, string connectId, SocketConnectOnlineType socketConnectOnlineType)
        {
            var socketConnect = new SocketConnect
            {
                UsersId = usersId,
                ConnectId = connectId,
                SocketConnectOnlineType = socketConnectOnlineType,
                LastDate = DateTime.Now
            };

            return await _socketConnectRepository.UpsertSocketConnectAsync(socketConnect);
        }

        /// <summary>
        /// 取得使用者連線資訊
        /// </summary>
        /// <param name="usersIds"></param>
        /// <returns></returns>
        public async Task<List<SocketConnect>> GetListSocketConnectByUsersIdsAsync(List<string> usersIds)
        {
            return await _socketConnectRepository.GetListSocketConnectByUsersIdsAsync(usersIds);
        }

        /// <summary>
        /// 取得使用者 By ConnectId
        /// </summary>
        /// <param name="connectId"></param>
        /// <returns></returns>
        public async Task<SocketConnect> GetListSocketConnectByConnectIdAsync(string connectId)
        {
            return await _socketConnectRepository.GetListSocketConnectByConnectIdsAsync(connectId);
        }

    }
}

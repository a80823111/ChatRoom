using Microsoft.AspNetCore.SignalR;
using Model.Enum.SocketConnectType;
using Model.Migrations;
using Model.ViewModels;
using Newtonsoft.Json;
using Service.Interfaces.IChat;
using Service.Interfaces.ISocket;
using Service.Interfaces.IUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IAuthService _authService;
        private readonly IChatService _chatService;
        private readonly ISocketService _socketService;
        private readonly IUsersService _usersService;

        public ChatHub(
            IAuthService authService,
            IChatService chatService,
            ISocketService socketService,
            IUsersService usersService)
        {
            _authService = authService;
            _chatService = chatService;
            _socketService = socketService;
            _usersService = usersService;
        }

        /// <summary>
        /// 連線成功
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            var token = Context.GetHttpContext().Request.Query["token"];
            //驗證登入
            if (!await _authService.AuthorizationAsync(Context.GetHttpContext(), token))
            {
                //強迫使用者斷開連接
                Context.Abort();
            }

            var claimsPrincipal = _authService.DecryptJwt(token);
            var loginUsersId = _authService.GetLoginUsersId(claimsPrincipal);

            if(!String.IsNullOrEmpty(loginUsersId))
            {
                //連線成功 , 紀錄ConnectId
                await _socketService.UpsertSocketConnectAsync(loginUsersId, Context.ConnectionId, SocketConnectOnlineType.Online);
            }
            


            await Clients.Caller.SendAsync("OnConnectedAsync",true);
        }

        /// <summary>
        /// 中斷連線
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {

            var socketConnect = await _socketService.GetListSocketConnectByConnectIdAsync(Context.ConnectionId);

            if(socketConnect != null)
            {
                //連線中斷 , 紀錄ConnectId
                await _socketService.UpsertSocketConnectAsync(socketConnect.UsersId, Context.ConnectionId, SocketConnectOnlineType.Offline);
            }
            await Clients.Caller.SendAsync("OnDisconnectedAsync", false);
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// 發送訊息內容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendChatContent(string input)
        {
            var sendChatContentViewModel = JsonConvert.DeserializeObject<SendChatContentViewModel>(input);


            var socketConnect = await _socketService.GetListSocketConnectByConnectIdAsync(Context.ConnectionId);
            var users = await _usersService.GetUsersByIdAsync(socketConnect.UsersId);

            sendChatContentViewModel.UsersId = users.Id;
            sendChatContentViewModel.NickName = users.NickName;

            var chatContent = await _chatService.SendChatContentAsync(sendChatContentViewModel);

            //取得群組成員
            var chatMembers = await _chatService.GetListChatRoomMemberAsync(sendChatContentViewModel.ChatRoom);

            //移除訊息發送者
            //chatMembers = chatMembers.Where(x => x != sendChatContentViewModel.UsersId).ToList();

            //public List<ChatContent> ChatContents { get; set; }
            //public List<Users> Users { get; set; }

            await SocketSendData("ReciveChatContent", chatMembers.Select(x => x.Id).ToList(), new { ChatContents = chatContent, Users  = users });
            
        }

        /// <summary>
        /// 中樞傳送資料
        /// </summary>
        /// <param name="method"></param>
        /// <param name="usersIds"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SocketSendData(string method ,List<string> usersIds,object message)
        {
            var socketConnects = await _socketService.GetListSocketConnectByUsersIdsAsync(usersIds);
            var connectIds = socketConnects.Select(x => x.ConnectId).ToList();

            await Clients.Clients(connectIds).SendAsync(method, JsonConvert.SerializeObject(message));
        }

        /// <summary>
        /// test
        /// </summary>
        /// <param name="method"></param>
        /// <param name="usersIds"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task test(string update)
        {
            await Clients.Caller.SendAsync("ReciveChatContent", update);
        }
    }
}

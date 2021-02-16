using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Model.BaseModels;
using Model.Enum.ApiResponseType;
using Model.ViewModels;
using Service.Interfaces.IChat;
using Service.Interfaces.IUsers;

namespace WebApi.Controllers
{
    [ApiController]
    public class ChatController : ControllerBase
    {
        
        private readonly IAuthService _authService;
        private readonly IChatService _chatService;
        private readonly IUsersService _usersService;


        public ChatController(IAuthService authService, IChatService chatService, IUsersService usersService)
        {
            _authService = authService;
            _chatService = chatService;
            _usersService = usersService;
        }

        /// <summary>
        /// 取得聊天室過去訊息
        /// </summary>
        /// <param name="chatroom">聊天室名稱</param>
        /// <returns></returns>
        [HttpGet, Route("api/Chat/FirstEnter/{chatroom}")]
        public async Task<ApiRes> GetChatContentWithFirstEnterAsync(string chatroom)
        {
            var apiResponse = new ApiRes();

            var loginUsersId = _authService.LoginUsersId(HttpContext);

            var roomMembers = await _chatService.GetListChatRoomMemberAsync(chatroom);

            //使用者第一次加入房間 , 新增訊息 & 同時會加入群組
            if (!roomMembers.Where(x => x.Id == loginUsersId).Any())
            {
                var users = await _usersService.GetUsersByIdAsync(loginUsersId);

                await _chatService.SendChatContentAsync(
                    new SendChatContentViewModel
                    {
                        ChatRoom = chatroom,
                        UsersId = loginUsersId,
                        Content = $"歡迎 {users.NickName} 已加入聊天 !"
                    });

            }



            var chatContentWithFirstEnter = await _chatService.GetChatContentWithFirstEnterAsync(chatroom);


            apiResponse.Status = ApiResType.Success;
            apiResponse.Result = chatContentWithFirstEnter;

            return apiResponse;
        }

        /// <summary>
        /// 取得使用者曾經進入過的聊天室
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/Chat/ChatRoomWithEnterBefore")]
        public async Task<ApiRes> GetListChatRoomWithEnterBeforeAsync()
        {
            var apiResponse = new ApiRes();

            var loginUsersId = _authService.LoginUsersId(HttpContext);

            var chatContentWithFirstEnter = await _chatService.GetListChatRoomWithEnterBeforeAsync(loginUsersId);

            apiResponse.Status = ApiResType.Success;
            apiResponse.Result = chatContentWithFirstEnter;

            return apiResponse;
        }

    }
}
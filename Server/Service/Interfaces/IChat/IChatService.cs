using Model.Migrations;
using Model.ResponseModels;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.IChat
{
    public interface IChatService
    {
        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="sendChatContentViewModel"></param>
        /// <returns></returns>
        Task<ChatContent> SendChatContentAsync(SendChatContentViewModel sendChatContentViewModel);

        /// <summary>
        /// 第一次進入聊天室取得所有訊息
        /// </summary>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
        Task<ChatContentWithFirstEnterResModel> GetChatContentWithFirstEnterAsync(string chatRoom);

        /// <summary>
        /// 取得使用者曾經進入過的聊天室
        /// </summary>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
        Task<List<ChatRoomWithEnterBeforeResModel>> GetListChatRoomWithEnterBeforeAsync(string usersId);

        /// <summary>
        /// 取得群組成員
        /// </summary>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
        Task<List<Users>> GetListChatRoomMemberAsync(string chatRoom);
    }
}

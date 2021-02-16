using Model.Migrations;
using Model.ResponseModels;
using Model.ViewModels;
using Repository.Repositories;
using Service.Interfaces.IChat;
using Service.Interfaces.IUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ChatService: IChatService
    {
        private readonly UsersRepository _userRepository;
        private readonly ChatMessageRepository _chatMessageRepository;

        public ChatService(UsersRepository userRepository, ChatMessageRepository chatMessageRepository)
        {
            _userRepository = userRepository;
            _chatMessageRepository = chatMessageRepository;
        }

        /// <summary>
        /// 發送訊息
        /// </summary>
        /// <param name="sendChatContentViewModel"></param>
        /// <returns></returns>
        public async Task<ChatContent> SendChatContentAsync(SendChatContentViewModel sendChatContentViewModel)
        {
            var chatContent = new ChatContent
            {
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
                UsersId = sendChatContentViewModel.UsersId,
                Content = sendChatContentViewModel.Content,
                CreateTime = DateTime.Now
            };

            var chatMessage = new ChatMessage
            {
                ChatRoom = sendChatContentViewModel.ChatRoom,
                ChatContents = new List<ChatContent> { chatContent },
                BuketDate = DateTime.Now
            };

            await _chatMessageRepository.UpsertAsync(chatMessage);

            return chatContent;
        }

        /// <summary>
        /// 第一次進入聊天室取得所有訊息
        /// </summary>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
        public async Task<ChatContentWithFirstEnterResModel> GetChatContentWithFirstEnterAsync(string chatRoom)
        {
            var chatMessages = await _chatMessageRepository.GetListChatMessageByChatRoomAsync(chatRoom);

            var chatContens = chatMessages.SelectMany(x => x.ChatContents).ToList();

            var usersIds = chatContens.Select(x => x.UsersId).Distinct().ToList();

            var users = await _userRepository.GetListUserAsync(usersIds);

            var result = new ChatContentWithFirstEnterResModel
            {
                ChatContents = chatContens,
                Users = users
            };

            return result;
        }

        /// <summary>
        /// 取得使用者曾經進入過的聊天室
        /// </summary>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
        public async Task<List<ChatRoomWithEnterBeforeResModel>> GetListChatRoomWithEnterBeforeAsync(string usersId)
        {
            var chatMessages = await _chatMessageRepository.GetListUsersEnterBeforeChatRoomAsync(usersId);

            var result = chatMessages.Select(x => new ChatRoomWithEnterBeforeResModel {
                ChatRoom = x.ChatRoom,
                UsersCount = x.ChatContents.Select(x => x.UsersId).Distinct().Count()
            }).ToList();

            return result;
        }


        /// <summary>
        /// 取得群組成員
        /// </summary>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
        public async Task<List<Users>> GetListChatRoomMemberAsync(string chatRoom)
        {
            var members = await _chatMessageRepository.GetListChatRoomMemberAsync(chatRoom);

            return await _userRepository.GetListUserAsync(members);
        }
    }

}

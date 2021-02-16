using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModels
{
    public class SendChatContentViewModel
    {
        /// <summary>
        /// 訊息發送者
        /// </summary>
        public string UsersId { get; set; }

        /// <summary>
        /// 使用者暱稱
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 聊天室
        /// </summary>
        public string ChatRoom { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public string Content { get; set; }
    }
}

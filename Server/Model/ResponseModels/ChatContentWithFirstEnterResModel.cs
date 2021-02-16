using Model.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ResponseModels
{
    public class ChatContentWithFirstEnterResModel
    {
        public List<ChatContent> ChatContents { get; set; }
        public List<Users> Users { get; set; }
    }
}

using System;

namespace MVC_DB_.Models
{
    public class FriendMessage
    {
        public int id { get; set; }

        public int friendID { get; set; }

        public int senderAccountID { get; set; }

        public string message { get; set; }

        public DateTime createdAt { get; set; }
    }
}
using System;

namespace MVC_DB_.Models
{
    public class Notification
    {
        public int id { get; set; }

        public int accountID { get; set; }

        public int fromAccountID { get; set; }

        public int portfolioID { get; set; }

        public string notificationType { get; set; }
        public bool isRead { get; set; }
        public DateTime createdAt { get; set; }


        // 顯示通知用
        public string fromUserName { get; set; }
        public string portfolioTitle { get; set; }


    }
}
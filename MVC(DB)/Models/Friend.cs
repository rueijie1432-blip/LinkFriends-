namespace MVC_DB_.Models
{
    public class Friend
    {
        public int id { get; set; }

        public int senderAccountID { get; set; }

        public int receiverAccountID { get; set; }

        public int status { get; set; }

        // 額外顯示用，不存入資料庫
        public string friendName { get; set; }
        public string friendAvatar { get; set; }

    }
}
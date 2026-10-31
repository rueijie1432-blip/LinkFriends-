using System.Data.SqlClient;
using System.Collections.Generic;

namespace MVC_DB_.Models
{
    public class FriendMessageDB
    {
        private readonly string connStr = Database.ConnStr;

        // CREATE
        public void CreateFriendMessage(FriendMessage friendMessage)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO FriendMessage(friendID, senderAccountID, message, createdAt) 
                                 VALUES(@friendID, @senderAccountID, @message, @createdAt)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@friendID", friendMessage.friendID);
                    sqlCommand.Parameters.AddWithValue("@senderAccountID", friendMessage.senderAccountID);
                    sqlCommand.Parameters.AddWithValue("@message", friendMessage.message);
                    sqlCommand.Parameters.AddWithValue("@createdAt", friendMessage.createdAt);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // READ
        public List<FriendMessage> GetFriendMessages(int friendID)
        {
            List<FriendMessage> friendMessages = new List<FriendMessage>();
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM FriendMessage WHERE friendID = @friendID ORDER BY createdAt ASC";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@friendID", friendID);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FriendMessage friendMessage = new FriendMessage
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                friendID = reader.GetInt32(reader.GetOrdinal("friendID")),
                                senderAccountID = reader.GetInt32(reader.GetOrdinal("senderAccountID")),
                                message = reader.GetString(reader.GetOrdinal("message")),
                                createdAt = reader.GetDateTime(reader.GetOrdinal("createdAt"))
                            };
                            friendMessages.Add(friendMessage);
                        }
                    }
                }
            }
            return friendMessages;
        }





    }
}

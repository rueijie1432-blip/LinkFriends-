using System.Data.SqlClient;
using System.Collections.Generic;

namespace MVC_DB_.Models
{
    public class FriendDB
    {
        private readonly string connStr = Database.ConnStr;

        // CREATE
        public void CreateFriend(Friend friend)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Friend(senderAccountID, receiverAccountID, status) 
                                 VALUES(@senderAccountID, @receiverAccountID, @status)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@senderAccountID", friend.senderAccountID);
                    sqlCommand.Parameters.AddWithValue("@receiverAccountID", friend.receiverAccountID);
                    sqlCommand.Parameters.AddWithValue("@status", friend.status);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // READ
        public List<Friend> GetFriends(int accountID)
        {
            List<Friend> friends = new List<Friend>();
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Friend WHERE senderAccountID = @accountID OR receiverAccountID = @accountID";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@accountID", accountID);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Friend friend = new Friend
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                senderAccountID = reader.GetInt32(reader.GetOrdinal("senderAccountID")),
                                receiverAccountID = reader.GetInt32(reader.GetOrdinal("receiverAccountID")),
                                status = reader.GetInt32(reader.GetOrdinal("status"))
                            };
                            friends.Add(friend);
                        }
                    }
                }
            }
            return friends;
        }

        // READ WHO SENT FRIEND REQUESTS TO A SPECIFIC ACCOUNT
        public List<Friend> GetFriendRequests(int accountID)
        {
            List<Friend> friendRequests = new List<Friend>();
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Friend WHERE receiverAccountID = @accountID AND status = 0";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@accountID", accountID);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Friend friend = new Friend
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                senderAccountID = reader.GetInt32(reader.GetOrdinal("senderAccountID")),
                                receiverAccountID = reader.GetInt32(reader.GetOrdinal("receiverAccountID")),
                                status = reader.GetInt32(reader.GetOrdinal("status"))
                            };
                            friendRequests.Add(friend);
                        }
                    }
                }
            }
            return friendRequests;
        }

        // UPDATE
        public void UpdateFriendStatus(int friendID, int newStatus, int accountID)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "UPDATE Friend SET status = @newStatus WHERE id = @friendID AND receiverAccountID = @accountID";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@newStatus", newStatus);
                    sqlCommand.Parameters.AddWithValue("@friendID", friendID);
                    sqlCommand.Parameters.AddWithValue("@accountID", accountID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // DELETE
        public void DeleteFriend(int friendID, int accountID)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"
                    DELETE FROM Friend
                    WHERE id = @friendID
                    AND (senderAccountID = @accountID 
                         OR receiverAccountID = @accountID)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@friendID", friendID);
                    sqlCommand.Parameters.AddWithValue("@accountID", accountID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // 顯示所有使用者 + 排除已經是朋友的使用者 + 排除自己 + 排除已經送出好友邀請的使用者 + 排除已經收到好友邀請的使用者
        public List<account> GetAllUsersExceptFriendsAndSelf(int accountID)
        {
            List<account> allUsers = new List<account>();
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"
                    SELECT * FROM account
                    WHERE id != @accountID
                    AND id NOT IN (
                        SELECT senderAccountID FROM Friend WHERE receiverAccountID = @accountID
                        UNION
                        SELECT receiverAccountID FROM Friend WHERE senderAccountID = @accountID
                    )
                    AND id NOT IN (
                        SELECT receiverAccountID FROM Friend WHERE senderAccountID = @accountID AND status = 0
                    )
                    AND id NOT IN (
                        SELECT senderAccountID FROM Friend WHERE receiverAccountID = @accountID AND status = 0
                    )";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@accountID", accountID);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            account user = new account
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                userName = reader.GetString(reader.GetOrdinal("userName")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                avatar = reader.IsDBNull(reader.GetOrdinal("avatar"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("avatar"))
                            };
                            allUsers.Add(user);
                        }
                    }
                }
            }
            return allUsers;
        }

    }
}

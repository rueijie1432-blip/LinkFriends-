using System.Data.SqlClient;
using System.Collections.Generic;

namespace MVC_DB_.Models
{
    public class NotificationDB
    {
        private readonly string connStr = Database.ConnStr;

        // CREATE
        public void CreateNotification(Notification notification)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"
                    INSERT INTO Notification
                    (accountID, fromAccountID, portfolioID, notificationType)
                    VALUES
                    (@accountID, @fromAccountID, @portfolioID, @notificationType)";

                using (SqlCommand sqlCommand =
                    new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue(
                        "@accountID", notification.accountID);

                    sqlCommand.Parameters.AddWithValue(
                        "@fromAccountID", notification.fromAccountID);

                    sqlCommand.Parameters.AddWithValue(
                        "@portfolioID", notification.portfolioID);

                    sqlCommand.Parameters.AddWithValue(
                        "@notificationType", notification.notificationType);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // READ
        public List<Notification> GetNotifications(int accountID)
        {
            List<Notification> notifications = new List<Notification>();

            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"
            SELECT 
                n.id,
                n.accountID,
                n.fromAccountID,
                n.portfolioID,
                n.notificationType,
                n.isRead,
                n.createdAt,
                a.userName AS fromUserName,
                p.title AS portfolioTitle
            FROM Notification n
            JOIN Account a ON n.fromAccountID = a.id
            JOIN Portfolio p ON n.portfolioID = p.id
            WHERE n.accountID = @accountID
            ORDER BY n.createdAt DESC";

                using (SqlCommand sqlCommand =
                    new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue(
                        "@accountID", accountID);

                    sqlConnection.Open();

                    using (SqlDataReader reader =
                        sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Notification notification = new Notification
                            {
                                id = reader.GetInt32(
                                    reader.GetOrdinal("id")),

                                accountID = reader.GetInt32(
                                    reader.GetOrdinal("accountID")),

                                fromAccountID = reader.GetInt32(
                                    reader.GetOrdinal("fromAccountID")),

                                portfolioID = reader.GetInt32(
                                    reader.GetOrdinal("portfolioID")),

                                notificationType = reader.GetString(
                                    reader.GetOrdinal("notificationType")),

                                isRead = reader.GetBoolean(
                                    reader.GetOrdinal("isRead")),

                                createdAt = reader.GetDateTime(
                                    reader.GetOrdinal("createdAt")),

                                fromUserName = reader.GetString(
                                    reader.GetOrdinal("fromUserName")),

                                portfolioTitle = reader.GetString(
                                    reader.GetOrdinal("portfolioTitle"))
                            };

                            notifications.Add(notification);
                        }
                    }
                }
            }

            return notifications;
        }


        // UPDATE
        public void MarkAllAsRead(int accountID)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"
                    UPDATE Notification
                    SET isRead = 1
                    WHERE accountID = @accountID
                    AND isRead = 0";

                using (SqlCommand sqlCommand =
                    new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue(
                        "@accountID", accountID);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
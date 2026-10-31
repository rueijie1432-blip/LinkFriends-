using System.Data.SqlClient;
using System.Collections.Generic;

namespace MVC_DB_.Models
{
    public class PortfolioCommentDB
    {
        private readonly string connStr = Database.ConnStr;

        // CREATE
        public int CreatePortfolioComment(PortfolioComment portfolioComment)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO PortfolioComment(comment, accountID, portfolioID) 
                                OUTPUT INSERTED.id
                                VALUES(@comment, @accountID, @portfolioID)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@comment", portfolioComment.comment);
                    sqlCommand.Parameters.AddWithValue("@accountID", portfolioComment.accountID);
                    sqlCommand.Parameters.AddWithValue("@portfolioID", portfolioComment.portfolioID);
                    sqlConnection.Open();

                    // -sqlCommand.ExecuteNonQuery();

                    return (int)sqlCommand.ExecuteScalar();
                }
            }
        }

        // READ all portfoliocomments
        public List<PortfolioComment> GetAllPortfolioComments()
        {
            List<PortfolioComment> comments = new List<PortfolioComment>();
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM PortfolioComment ORDER BY id DESC";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PortfolioComment comment = new PortfolioComment
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                comment = reader.GetString(reader.GetOrdinal("comment")),
                                accountID = reader.GetInt32(reader.GetOrdinal("accountID")),
                                portfolioID = reader.GetInt32(reader.GetOrdinal("portfolioID")),
                                isBlocked = reader.GetBoolean(reader.GetOrdinal("isBlocked"))
                            };
                            comments.Add(comment);
                        }
                    }
                }
            }
            return comments;
        }

        // READ all comments for a portfolio
        public List<PortfolioComment> GetPortfolioComments(int portfolioID)
        {
            List<PortfolioComment> comments = new List<PortfolioComment>();
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM PortfolioComment WHERE portfolioID = @portfolioID ORDER BY id DESC";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@portfolioID", portfolioID);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PortfolioComment comment = new PortfolioComment
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                comment = reader.GetString(reader.GetOrdinal("comment")),
                                accountID = reader.GetInt32(reader.GetOrdinal("accountID")),
                                portfolioID = reader.GetInt32(reader.GetOrdinal("portfolioID")),
                                isBlocked = reader.GetBoolean(reader.GetOrdinal("isBlocked"))
                            };
                            comments.Add(comment);
                        }
                    }
                }
            }
            return comments;
        }

        // READ a single comment by ID
        public PortfolioComment GetPortfolioCommentById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM PortfolioComment WHERE id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PortfolioComment
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                comment = reader.GetString(reader.GetOrdinal("comment")),
                                accountID = reader.GetInt32(reader.GetOrdinal("accountID")),
                                portfolioID = reader.GetInt32(reader.GetOrdinal("portfolioID")),
                                isBlocked = reader.GetBoolean(reader.GetOrdinal("isBlocked"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // DELETE a comment
        public void DeletePortfolioComment(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "DELETE FROM PortfolioComment WHERE id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // SetPortfolioCommentBlock
        public void SetPortfolioCommentBlock(int id, bool isBlocked)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "UPDATE PortfolioComment SET isBlocked = @isBlocked WHERE id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@isBlocked", isBlocked);
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

    }
}

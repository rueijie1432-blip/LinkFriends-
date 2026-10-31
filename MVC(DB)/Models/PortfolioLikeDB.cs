using System.Data.SqlClient;
using System.Collections.Generic;

namespace MVC_DB_.Models
{
    public class PortfolioLikeDB
    {
        private readonly string connStr = Database.ConnStr;

        // CREATE
        public void CreatePortfolioLike(PortfolioLike portfolioLike)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO PortfolioLike(portfolioID, accountID) 
                                VALUES(@portfolioID, @accountID)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@portfolioID", portfolioLike.portfolioID);
                    sqlCommand.Parameters.AddWithValue("@accountID", portfolioLike.accountID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // Read likes count for a portfolio
        public int GetPortfolioLikesCount(int portfolioID)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT COUNT(*) FROM PortfolioLike WHERE portfolioID = @portfolioID";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@portfolioID", portfolioID);
                    sqlConnection.Open();
                    return (int)sqlCommand.ExecuteScalar();
                }
            }
        }

        // Read likes for a portfolio and account
        public PortfolioLike GetPortfolioLike(int portfolioID, int accountID)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM PortfolioLike WHERE portfolioID = @portfolioID AND accountID = @accountID";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@portfolioID", portfolioID);
                    sqlCommand.Parameters.AddWithValue("@accountID", accountID);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PortfolioLike
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                portfolioID = reader.GetInt32(reader.GetOrdinal("portfolioID")),
                                accountID = reader.GetInt32(reader.GetOrdinal("accountID"))
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        // DELETE
        public void DeletePortfolioLike(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "DELETE FROM PortfolioLike WHERE id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }




    }
}
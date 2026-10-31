using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MVC_DB_.Models
{
    public class PortfolioDB
    {
        private readonly string connStr = Database.ConnStr;

        // CREATE
        public void CreatePortfolio(Portfolio portfolio)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO Portfolio(title, description, image, accountID) 
                                VALUES(@title, @description, @image, @accountID)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@title", portfolio.title);
                    sqlCommand.Parameters.AddWithValue("@description", portfolio.description);
                    sqlCommand.Parameters.AddWithValue(
                        "@image",
                        (object)portfolio.image ?? DBNull.Value
                    );
                    sqlCommand.Parameters.AddWithValue("@accountID", portfolio.accountID);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // READ
        public List<Portfolio> GetPortfolios(int accountID)
        {
            List<Portfolio> portfolios = new List<Portfolio>();
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Portfolio WHERE accountID = @accountID";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@accountID", accountID);

                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Portfolio portfolio = new Portfolio
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                title = reader.GetString(reader.GetOrdinal("title")),
                                description = reader.GetString(reader.GetOrdinal("description")),
                                image = reader.IsDBNull(reader.GetOrdinal("image"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("image")),
                                accountID = reader.GetInt32(reader.GetOrdinal("accountID")),
                                isBlocked = reader.GetBoolean(reader.GetOrdinal("isBlocked"))
                            };
                            portfolios.Add(portfolio);
                        }
                    }
                }
            }
            return portfolios;
        }

        // UPDATE
        public void UpdatePortfolio(Portfolio portfolio)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"UPDATE Portfolio 
                                 SET title = @title, description = @description
                                 WHERE id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@title", portfolio.title);
                    sqlCommand.Parameters.AddWithValue("@description", portfolio.description);
                    sqlCommand.Parameters.AddWithValue("@id", portfolio.id);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // DELETE
        public void DeletePortfolio(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "DELETE FROM Portfolio WHERE id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // Get a portfolio by id
        public Portfolio GetPortfolioById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Portfolio WHERE id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Portfolio portfolio = new Portfolio
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                title = reader.GetString(reader.GetOrdinal("title")),
                                description = reader.GetString(reader.GetOrdinal("description")),
                                image = reader.IsDBNull(reader.GetOrdinal("image"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("image")),
                                accountID = reader.GetInt32(reader.GetOrdinal("accountID")),
                                isBlocked = reader.GetBoolean(reader.GetOrdinal("isBlocked"))
                            };
                            return portfolio;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        // Get all portfolios
        public List<Portfolio> GetAllPortfolios()
        {
            List<Portfolio> portfolios = new List<Portfolio>();
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM Portfolio ORDER BY id DESC";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Portfolio portfolio = new Portfolio
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                title = reader.GetString(reader.GetOrdinal("title")),
                                description = reader.GetString(reader.GetOrdinal("description")),
                                image = reader.IsDBNull(reader.GetOrdinal("image"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("image")),
                                accountID = reader.GetInt32(reader.GetOrdinal("accountID")),
                                isBlocked = reader.GetBoolean(reader.GetOrdinal("isBlocked"))

                            };
                            portfolios.Add(portfolio);
                        }
                    }
                }
            }
            return portfolios;
        }

        // SetPortfolioBlock
        public void SetPortfolioBlock(int id, bool isBlocked)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "UPDATE Portfolio SET isBlocked = @isBlocked WHERE id = @id";
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

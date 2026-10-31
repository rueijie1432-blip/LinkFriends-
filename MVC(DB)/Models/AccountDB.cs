using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MVC_DB_.Models
{
    public class AccountDB
    {
        private readonly string connStr = Database.ConnStr;

        // CREATE
        public void CreateAccount(account user)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"INSERT INTO account(userName, passwd, name, avatar) 
                                 VALUES(@userName, @passwd, @name, @avatar)";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@userName", user.userName);
                    sqlCommand.Parameters.AddWithValue("@passwd", user.passwd);
                    sqlCommand.Parameters.AddWithValue("@name", user.name);
                    sqlCommand.Parameters.AddWithValue("@avatar", "default-avatar.png");
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // READ
        public List<account> GetAccounts()
        {
            List<account> accounts = new List<account>();
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = "SELECT * FROM account";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            account user = new account
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                userName = reader.GetString(reader.GetOrdinal("userName")),
                                passwd = reader.GetString(reader.GetOrdinal("passwd")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                avatar = reader.IsDBNull(reader.GetOrdinal("avatar"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("avatar")),
                                isAdmin = reader.GetBoolean(reader.GetOrdinal("isAdmin"))
                            };
                            accounts.Add(user);
                        }
                    }
                }
            }
            return accounts;
        }

        // UPDATE
        public void UpdateAccount(account user)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"UPDATE account
                                 SET name = @name,
                                     avatar = @avatar
                                 WHERE id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", user.id);
                    sqlCommand.Parameters.AddWithValue("@name", user.name);
                    sqlCommand.Parameters.AddWithValue("@avatar", user.avatar);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        // Get account by ID
        public account GetAccountById(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"SELECT *
                                 FROM account
                                 WHERE id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            account user = new account
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                userName = reader.GetString(reader.GetOrdinal("userName")),
                                passwd = reader.GetString(reader.GetOrdinal("passwd")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                avatar = reader.IsDBNull(reader.GetOrdinal("avatar"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("avatar"))
                            };
                            return user;
                        }
                    }
                }
            }
            return null;
        }

        // Login
        public account Login(string userName, string passwd)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"SELECT *
                         FROM account
                         WHERE userName = @userName
                         AND passwd = @passwd";

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@userName", userName);
                    sqlCommand.Parameters.AddWithValue("@passwd", passwd);

                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            account user = new account
                            {
                                id = reader.GetInt32(reader.GetOrdinal("id")),
                                userName = reader.GetString(reader.GetOrdinal("userName")),
                                passwd = reader.GetString(reader.GetOrdinal("passwd")),
                                name = reader.GetString(reader.GetOrdinal("name")),
                                avatar = reader.IsDBNull(reader.GetOrdinal("avatar"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("avatar"))
                            };

                            return user;
                        }
                    }
                }
            }

            return null;
        }

        // isAdmin
        public bool IsAdmin(int accountId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connStr))
            {
                string query = @"SELECT isAdmin
                                 FROM account
                                 WHERE id = @id";
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", accountId);

                    sqlConnection.Open();

                    return Convert.ToBoolean(sqlCommand.ExecuteScalar());
                }
            }
        }


    }
}
using Microsoft.Data.SqlClient;
using System.Text;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static class User
        {
            #region Create
            public static bool CreateUser(string username, string password, string discriminator)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append($@"IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '{username}')");
                        query.AppendLine($@"BEGIN");
                        query.AppendLine($@"CREATE LOGIN {username}");
                        query.AppendLine($@"WITH PASSWORD = '{password}'");
                        query.AppendLine($@"END");

                        query.AppendLine($@"BEGIN");
                        query.AppendLine($@"CREATE USER {username} FOR LOGIN {username}");
                        query.AppendLine($@"END");

                        query.AppendLine($@"BEGIN");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.AuctionBids} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Auctions} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Base} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Bus} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.CorporateUser} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.HeavyVehicle} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.PassengerCar} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.PrivatePassengerCar} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.PrivateUser} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.ProfessionalPassengerCar} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Truck} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Users} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.VehicleImages} TO {username}");
                        query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Vehicles} TO {username}");
                        query.AppendLine($@"GRANT CREATE PROCEDURE TO {username}");
                        query.AppendLine($@"GRANT CREATE VIEW TO {username}");
                        query.AppendLine($@"END");

                        ExecuteNonQuery(query.ToString());

                        int baseId = Base.InsertIntoBase(con);

                        StringBuilder Userquery = new StringBuilder();
                        Userquery.Append($@"INSERT INTO {DatabaseTables.Users} ");
                        Userquery.Append($@"({nameof(UserModels.User.UserName)},");
                        Userquery.Append($@"{nameof(UserModels.User.Discriminator)},");
                        Userquery.Append($@"{nameof(UserModels.User.BaseId)})");
                        Userquery.Append($@" VALUES ");
                        Userquery.Append($@"(@{nameof(UserModels.User.UserName)},");
                        Userquery.Append($@"@{nameof(UserModels.User.Discriminator)},");
                        Userquery.Append($@"@{nameof(UserModels.User.BaseId)})");

                        using (SqlCommand cmd = new SqlCommand(Userquery.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.User.UserName)}", username);
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.User.Discriminator)}", discriminator);
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.User.BaseId)}", baseId);

                            cmd.ExecuteNonQuery();
                        }

                        con.Close();
                    }
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }

            public static bool CreatePrivateUser(string username, string cprnumber)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        int userId = GetUserId(username, con);

                        StringBuilder query = new StringBuilder();
                        query.Append($@"INSERT INTO {DatabaseTables.PrivateUser} ");
                        query.Append($@"({nameof(UserModels.PrivateUser.UserId)},");
                        query.Append($@"{nameof(UserModels.PrivateUser.CPRNumber)})");
                        query.Append($@" VALUES ");
                        query.Append($@"(@{nameof(UserModels.PrivateUser.UserId)},");
                        query.Append($@"@{nameof(UserModels.PrivateUser.CPRNumber)})");

                        using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.PrivateUser.UserId)}", userId);
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.PrivateUser.CPRNumber)}", cprnumber);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }

            public static bool CreateCorporateUser(string username, long credit, string cvrnumber)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        int userId = GetUserId(username, con);

                        StringBuilder query = new StringBuilder();
                        query.Append($@"INSERT INTO {DatabaseTables.CorporateUser} ");
                        query.Append($@"({nameof(UserModels.CorporateUser.UserId)},");
                        query.Append($@"{nameof(UserModels.CorporateUser.Credit)},");
                        query.Append($@"{nameof(UserModels.CorporateUser.CvrNumber)})");
                        query.Append($@" VALUES ");
                        query.Append($@"(@{nameof(UserModels.CorporateUser.UserId)},");
                        query.Append($@"@{nameof(UserModels.CorporateUser.Credit)},");
                        query.Append($@"@{nameof(UserModels.CorporateUser.CvrNumber)})");

                        using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.UserId)}", userId);
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.Credit)}", credit);
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.CvrNumber)}", cvrnumber);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
            #endregion

            #region Read
            public static UserModels.PrivateUser GetPrivateUser(string username)
            {
                UserModels.PrivateUser user = new UserModels.PrivateUser();
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append($@"SELECT * FROM {DatabaseTables.Users} WHERE {nameof(UserModels.User.UserName)} = @{nameof(UserModels.User.UserName)}");

                        using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.User.UserName)}", username);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    user.UserId = (int)reader[nameof(UserModels.PrivateUser.UserId)];
                                    user.UserName = reader[nameof(UserModels.PrivateUser.UserName)].ToString();
                                    user.Discriminator = reader[nameof(UserModels.PrivateUser.Discriminator)].ToString();
                                    user.BaseId = (int)reader[nameof(UserModels.PrivateUser.BaseId)];
                                    user.Status = Base.GetStatus(user.BaseId, con);
                                    user.CreatedAt = Base.GetCreatedAt(user.BaseId, con);
                                    user.UpdatedAt = Base.GetUpdatedAt(user.BaseId, con);
                                    user.DeletedAt = Base.GetDeletedAt(user.BaseId, con);
                                    user.CPRNumber = GetCPRNumber(user.UserId, con);
                                }
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    return user;
                }
                return user;
            }

            public static UserModels.CorporateUser GetCorporateUser(string username)
            {
                UserModels.CorporateUser user = new UserModels.CorporateUser();
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        StringBuilder query = new StringBuilder();
                        query.Append($@"SELECT * FROM {DatabaseTables.Users} WHERE {nameof(UserModels.User.UserName)} = @{nameof(UserModels.User.UserName)}");

                        using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.User.UserName)}", username);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    user.UserId = (int)reader[nameof(UserModels.CorporateUser.UserId)];
                                    user.UserName = reader[nameof(UserModels.CorporateUser.UserName)].ToString();
                                    user.Discriminator = reader[nameof(UserModels.CorporateUser.Discriminator)].ToString();
                                    user.BaseId = (int)reader[nameof(UserModels.CorporateUser.BaseId)];
                                    user.Status = Base.GetStatus(user.BaseId, con);
                                    user.CreatedAt = Base.GetCreatedAt(user.BaseId, con);
                                    user.UpdatedAt = Base.GetUpdatedAt(user.BaseId, con);
                                    user.DeletedAt = Base.GetDeletedAt(user.BaseId, con);
                                    user.CvrNumber = GetCVRNumber(user.UserId, con);
                                    user.Credit = GetCredit(user.UserId, con);
                                }
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    return user;
                }
                return user;
            }
            #endregion

            #region Update
            public static bool UpdatePrivateUser(string username, string cprnumber)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        int userId = GetUserId(username, con);

                        StringBuilder query = new StringBuilder();
                        query.Append($@"UPDATE {DatabaseTables.PrivateUser} SET ");
                        query.Append($"{nameof(UserModels.PrivateUser.CPRNumber)} = @{nameof(UserModels.PrivateUser.CPRNumber)} ");
                        query.Append($"WHERE {nameof(UserModels.PrivateUser.UserId)} = @{nameof(UserModels.PrivateUser.UserId)}");

                        using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.PrivateUser.CPRNumber)}", cprnumber);
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.PrivateUser.UserId)}", userId);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }

            public static bool UpdateCorporateUser(string username, long credit, string cvrnumber)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        int userId = GetUserId(username, con);

                        StringBuilder query = new StringBuilder();
                        query.Append($@"UPDATE {DatabaseTables.CorporateUser} SET ");
                        query.Append($"{nameof(UserModels.CorporateUser.Credit)} = @{nameof(UserModels.CorporateUser.Credit)}, ");
                        query.Append($"{nameof(UserModels.CorporateUser.CvrNumber)} = @{nameof(UserModels.CorporateUser.CvrNumber)} ");
                        query.Append($"WHERE {nameof(UserModels.CorporateUser.UserId)} = @{nameof(UserModels.CorporateUser.UserId)}");

                        using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.Credit)}", credit);
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.CvrNumber)}", cvrnumber);
                            cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.UserId)}", userId);

                            cmd.ExecuteNonQuery();
                        }

                        con.Close();
                    }
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
            #endregion

            #region Internal
            private static string GetCPRNumber(int userId, SqlConnection con)
            {
                string? cprNumber = string.Empty;
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.Append($@"SELECT * FROM {DatabaseTables.PrivateUser} WHERE {nameof(UserModels.PrivateUser.UserId)} = @{nameof(UserModels.PrivateUser.UserId)}");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(UserModels.PrivateUser.UserId)}", userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cprNumber = reader[nameof(UserModels.PrivateUser.CPRNumber)].ToString();
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    return cprNumber;
                }
                return cprNumber;
            }

            private static string GetCVRNumber(int userId, SqlConnection con)
            {
                string? cvrNumber = string.Empty;
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.Append($@"SELECT * FROM {DatabaseTables.CorporateUser} WHERE {nameof(UserModels.CorporateUser.UserId)} = @{nameof(UserModels.CorporateUser.UserId)}");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.UserId)}", userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cvrNumber = reader[nameof(UserModels.CorporateUser.CvrNumber)].ToString();
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    return cvrNumber;
                }
                return cvrNumber;
            }

            private static long GetCredit(int userId, SqlConnection con)
            {
                long credit = 0;
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.Append($@"SELECT * FROM {DatabaseTables.CorporateUser} WHERE {nameof(UserModels.CorporateUser.UserId)} = @{nameof(UserModels.CorporateUser.UserId)}");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.UserId)}", userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                credit = (long)reader[nameof(UserModels.CorporateUser.Credit)];
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    return credit;
                }
                return credit;
            }

            private static int GetUserId(string username, SqlConnection con)
            {
                int userId = 0;
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.Append($@"SELECT * FROM {DatabaseTables.Users} WHERE {nameof(UserModels.User.UserName)} = @{nameof(UserModels.User.UserName)}");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(UserModels.User.UserName)}", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userId = (int)reader[nameof(UserModels.User.Id)];
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    return userId;
                }
                return userId;
            }
            #endregion
        }
    }
}
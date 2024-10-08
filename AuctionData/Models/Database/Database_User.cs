﻿using AuctionData.Models.UserModels;
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
                using (SqlConnection con = OpenNewConnection())
                {
                    using (SqlTransaction transaction = con.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder query = new StringBuilder();
                            query.Append($@"IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '{username}')");
                            query.AppendLine($@" BEGIN ");
                            query.AppendLine($@"CREATE LOGIN {username} WITH PASSWORD = '{password}'");
                            query.AppendLine($@" END");

                            query.AppendLine($@" BEGIN ");
                            query.AppendLine($@"CREATE USER {username} FOR LOGIN {username}");
                            query.AppendLine($@" END");

                            query.AppendLine($@" BEGIN ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.AuctionBids} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Auctions} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Base} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Bus} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.CorporateUser} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.HeavyVehicle} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.PassengerCar} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.PrivatePassengerCar} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.ProfessionalPassengerCar} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Truck} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Users} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.VehicleImages} TO {username} ");
                            query.AppendLine($@"GRANT INSERT, UPDATE ON {DatabaseTables.Vehicles} TO {username} ");
                            query.AppendLine($@"END");

                            using (SqlCommand cmd = new SqlCommand(query.ToString(), con, transaction))
                            {
                                cmd.ExecuteNonQuery();
                            }

                            int baseId = Base.InsertIntoBase(con, transaction);
                            string insertUserQuery = $@"
                                INSERT INTO {DatabaseTables.Users} 
                                ({nameof(UserModels.User.UserName)}, {nameof(UserModels.User.Discriminator)}, {nameof(UserModels.User.BaseId)})
                                VALUES (@UserName, @Discriminator, @BaseId)";

                            int userId;
                            using (SqlCommand cmd = new SqlCommand(insertUserQuery, con, transaction))
                            {
                                cmd.Parameters.AddWithValue("@UserName", username);
                                cmd.Parameters.AddWithValue("@Discriminator", discriminator);
                                cmd.Parameters.AddWithValue("@BaseId", baseId);
                                cmd.ExecuteNonQuery();

                                userId = GetUserId(username, con, transaction);
                            }

                            if (discriminator == "PrivateUser")
                            {
                                string insertPrivateUserQuery = $@"
                                    INSERT INTO {DatabaseTables.PrivateUser} 
                                    ({nameof(UserModels.PrivateUser.UserId)})
                                    VALUES (@UserId)";

                                using (SqlCommand cmd = new SqlCommand(insertPrivateUserQuery, con, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@UserId", userId);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            else if (discriminator == "CorporateUser")
                            {
                                string insertCorporateUserQuery = $@"
                                    INSERT INTO {DatabaseTables.CorporateUser} 
                                    ({nameof(UserModels.CorporateUser.UserId)}, {nameof(UserModels.CorporateUser.Credit)})
                                    VALUES (@UserId, @Credit)";

                                using (SqlCommand cmd = new SqlCommand(insertCorporateUserQuery, con, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@UserId", userId);
                                    cmd.Parameters.AddWithValue("@Credit", 0);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            transaction.Commit();
                            log.Info($"Creating user {username}");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            log.Error($"Could not create user {username}", ex);
                            transaction.Rollback();
                            return false;
                        }
                    }
                }
            }
            #endregion

            #region Read
            public static UserModels.User GetUser(string? username)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        string query = $@"
                            SELECT u.*, b.{nameof(Models.Base.CreatedAt)}, b.{nameof(Models.Base.UpdatedAt)}, b.{nameof(Models.Base.DeletedAt)}, b.{nameof(Models.Base.Status)},
                                pu.{nameof(UserModels.PrivateUser.CPRNumber)}, 
                                cu.{nameof(UserModels.CorporateUser.Credit)}, cu.{nameof(UserModels.CorporateUser.CvrNumber)}
                            FROM {DatabaseTables.Users} u
                            LEFT JOIN {DatabaseTables.PrivateUser} pu ON u.{nameof(UserModels.User.Id)} = pu.{nameof(UserModels.PrivateUser.UserId)}
                            LEFT JOIN {DatabaseTables.CorporateUser} cu ON u.{nameof(UserModels.User.Id)} = cu.{nameof(UserModels.CorporateUser.UserId)}
                            LEFT JOIN {DatabaseTables.Base} b ON u.{nameof(UserModels.User.BaseId)} = b.{nameof(Models.Base.Id)}
                            WHERE u.{nameof(UserModels.User.UserName)} = @UserName";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@UserName", username);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string? discriminator = reader[nameof(UserModels.User.Discriminator)].ToString();
                                    UserModels.User user;

                                    if (discriminator == "PrivateUser")
                                    {
                                        user = new UserModels.PrivateUser
                                        {
                                            CPRNumber = reader[nameof(UserModels.PrivateUser.CPRNumber)].ToString()
                                        };
                                    }
                                    else if (discriminator == "CorporateUser")
                                    {
                                        user = new UserModels.CorporateUser
                                        {
                                            Credit = Convert.ToDecimal(reader[nameof(UserModels.CorporateUser.Credit)]),
                                            CvrNumber = reader[nameof(UserModels.CorporateUser.CvrNumber)].ToString()
                                        };
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException("Unknown user type.");
                                    }

                                    user.Id = (int)reader[nameof(UserModels.User.Id)];
                                    user.UserName = reader[nameof(UserModels.User.UserName)].ToString();
                                    user.Discriminator = reader[nameof(UserModels.User.Discriminator)].ToString();
                                    user.PostalCode = reader[nameof(UserModels.User.PostalCode)].ToString();
                                    user.ProfileImage = reader[nameof(UserModels.User.ProfileImage)].ToString();
                                    user.BaseId = (int)reader[nameof(UserModels.User.BaseId)];
                                    user.CreatedAt = (DateTime)reader[nameof(Models.Base.CreatedAt)];
                                    user.UpdatedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.UpdatedAt))) ? null : (DateTime)reader[nameof(Models.Base.UpdatedAt)];
                                    user.DeletedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.DeletedAt))) ? null : (DateTime)reader[nameof(Models.Base.DeletedAt)];
                                    user.Status = (BaseStatus)reader[nameof(Models.Base.Status)];

                                    return user;
                                }
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    return null;
                }

                return null;
            }

            public static UserModels.User GetUser(int userId)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        string query = $@"
                            SELECT u.*, b.{nameof(Models.Base.CreatedAt)}, b.{nameof(Models.Base.UpdatedAt)}, b.{nameof(Models.Base.DeletedAt)}, b.{nameof(Models.Base.Status)},
                                pu.{nameof(UserModels.PrivateUser.CPRNumber)}, 
                                cu.{nameof(UserModels.CorporateUser.Credit)}, cu.{nameof(UserModels.CorporateUser.CvrNumber)}
                            FROM {DatabaseTables.Users} u
                            LEFT JOIN {DatabaseTables.PrivateUser} pu ON u.{nameof(UserModels.User.Id)} = pu.{nameof(UserModels.PrivateUser.UserId)}
                            LEFT JOIN {DatabaseTables.CorporateUser} cu ON u.{nameof(UserModels.User.Id)} = cu.{nameof(UserModels.CorporateUser.UserId)}
                            LEFT JOIN {DatabaseTables.Base} b ON u.{nameof(UserModels.User.BaseId)} = b.{nameof(Models.Base.Id)}
                            WHERE u.{nameof(UserModels.User.Id)} = @userId";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@userId", userId);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string? discriminator = reader[nameof(UserModels.User.Discriminator)].ToString();
                                    UserModels.User user;

                                    if (discriminator == "PrivateUser")
                                    {
                                        user = new UserModels.PrivateUser
                                        {
                                            CPRNumber = reader[nameof(UserModels.PrivateUser.CPRNumber)].ToString()
                                        };
                                    }
                                    else if (discriminator == "CorporateUser")
                                    {
                                        user = new UserModels.CorporateUser
                                        {
                                            Credit = Convert.ToDecimal(reader[nameof(UserModels.CorporateUser.Credit)]),
                                            CvrNumber = reader[nameof(UserModels.CorporateUser.CvrNumber)].ToString()
                                        };
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException("Unknown user type.");
                                    }

                                    user.Id = (int)reader[nameof(UserModels.User.Id)];
                                    user.UserName = reader[nameof(UserModels.User.UserName)].ToString();
                                    user.Discriminator = reader[nameof(UserModels.User.Discriminator)].ToString();
                                    user.BaseId = (int)reader[nameof(UserModels.User.BaseId)];
                                    user.ProfileImage = reader[nameof(UserModels.User.ProfileImage)].ToString();
                                    user.CreatedAt = (DateTime)reader[nameof(Models.Base.CreatedAt)];
                                    user.UpdatedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.UpdatedAt))) ? null : (DateTime)reader[nameof(Models.Base.UpdatedAt)];
                                    user.DeletedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.DeletedAt))) ? null : (DateTime)reader[nameof(Models.Base.DeletedAt)];
                                    user.Status = (BaseStatus)reader[nameof(Models.Base.Status)];

                                    return user;
                                }
                            }
                        }
                    }
                }
                catch (SqlException)
                {
                    return null;
                }

                return null;
            }

            public static UserModels.User GetUser(int userId, SqlConnection con)
            {
                try
                {
                    string query = $@"
                            SELECT u.*, b.{nameof(Models.Base.CreatedAt)}, b.{nameof(Models.Base.UpdatedAt)}, b.{nameof(Models.Base.DeletedAt)}, b.{nameof(Models.Base.Status)},
                                pu.{nameof(UserModels.PrivateUser.CPRNumber)}, 
                                cu.{nameof(UserModels.CorporateUser.Credit)}, cu.{nameof(UserModels.CorporateUser.CvrNumber)}
                            FROM {DatabaseTables.Users} u
                            LEFT JOIN {DatabaseTables.PrivateUser} pu ON u.{nameof(UserModels.User.Id)} = pu.{nameof(UserModels.PrivateUser.UserId)}
                            LEFT JOIN {DatabaseTables.CorporateUser} cu ON u.{nameof(UserModels.User.Id)} = cu.{nameof(UserModels.CorporateUser.UserId)}
                            LEFT JOIN {DatabaseTables.Base} b ON u.{nameof(UserModels.User.BaseId)} = b.{nameof(Models.Base.Id)}
                            WHERE u.{nameof(UserModels.User.Id)} = @userId";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string? discriminator = reader[nameof(UserModels.User.Discriminator)].ToString();
                                UserModels.User user;

                                if (discriminator == "PrivateUser")
                                {
                                    user = new UserModels.PrivateUser
                                    {
                                        CPRNumber = reader[nameof(UserModels.PrivateUser.CPRNumber)].ToString()
                                    };
                                }
                                else if (discriminator == "CorporateUser")
                                {
                                    user = new UserModels.CorporateUser
                                    {
                                        Credit = Convert.ToDecimal(reader[nameof(UserModels.CorporateUser.Credit)]),
                                        CvrNumber = reader[nameof(UserModels.CorporateUser.CvrNumber)].ToString()
                                    };
                                }
                                else
                                {
                                    throw new InvalidOperationException("Unknown user type.");
                                }

                                user.Id = (int)reader[nameof(UserModels.User.Id)];
                                user.UserName = reader[nameof(UserModels.User.UserName)].ToString();
                                user.Discriminator = reader[nameof(UserModels.User.Discriminator)].ToString();
                                user.BaseId = (int)reader[nameof(UserModels.User.BaseId)];
                                user.ProfileImage = reader[nameof(UserModels.User.ProfileImage)].ToString();
                                user.CreatedAt = (DateTime)reader[nameof(Models.Base.CreatedAt)];
                                user.UpdatedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.UpdatedAt))) ? null : (DateTime)reader[nameof(Models.Base.UpdatedAt)];
                                user.DeletedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.DeletedAt))) ? null : (DateTime)reader[nameof(Models.Base.DeletedAt)];
                                user.Status = (BaseStatus)reader[nameof(Models.Base.Status)];

                                return user;
                            }
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    log.Error("Could not retrieve user", ex);
                    return null;
                }
            }
            #endregion

            #region Update
            public static bool UpdatePrivateUser(PrivateUser user)
            {
                using (SqlConnection con = OpenNewConnection())
                {
                    using (SqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder userQuery = new StringBuilder();
                            userQuery.AppendLine($@"BEGIN ");
                            userQuery.Append($@"UPDATE {DatabaseTables.PrivateUser} SET ");
                            userQuery.Append($@"{nameof(UserModels.PrivateUser.CPRNumber)} = @{nameof(UserModels.PrivateUser.CPRNumber)}");
                            userQuery.AppendLine($@" WHERE {nameof(UserModels.PrivateUser.UserId)} = @{nameof(UserModels.PrivateUser.UserId)}");
                            userQuery.AppendLine($@" END ");
                            userQuery.AppendLine($@"BEGIN ");
                            userQuery.Append($@"UPDATE {DatabaseTables.Users} SET ");
                            userQuery.Append($@"{nameof(UserModels.PrivateUser.ProfileImage)} = @{nameof(UserModels.PrivateUser.ProfileImage)}, ");
                            userQuery.Append($@"{nameof(UserModels.PrivateUser.PostalCode)} = @{nameof(UserModels.PrivateUser.PostalCode)}");
                            userQuery.AppendLine($@" WHERE {nameof(UserModels.PrivateUser.Id)} = @{nameof(UserModels.PrivateUser.Id)}");
                            userQuery.Append($@" END");

                            using (SqlCommand cmd = new SqlCommand(userQuery.ToString(), con, trans))
                            {
                                cmd.Parameters.AddWithValue($"@{nameof(UserModels.PrivateUser.CPRNumber)}", user.CPRNumber);
                                cmd.Parameters.AddWithValue($"@{nameof(UserModels.User.Id)}", user.Id);
                                cmd.Parameters.AddWithValue($"@{nameof(UserModels.PrivateUser.PostalCode)}", user.PostalCode);
                                cmd.Parameters.AddWithValue($"@{nameof(UserModels.PrivateUser.ProfileImage)}", user.ProfileImage);
                                cmd.Parameters.AddWithValue($"@{nameof(UserModels.PrivateUser.UserId)}", user.UserId);

                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            log.Error("Something went wrong when trying to update private user", ex);
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }

            public static bool UpdateCorporatedUser(CorporateUser user)
            {
                using (SqlConnection con = OpenNewConnection())
                {
                    using (SqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder userQuery = new StringBuilder();
                            userQuery.AppendLine($@"BEGIN ");
                            userQuery.Append($@"UPDATE {DatabaseTables.CorporateUser} SET ");
                            userQuery.Append($@"{nameof(UserModels.CorporateUser.CvrNumber)} = @{nameof(UserModels.CorporateUser.CvrNumber)}");
                            userQuery.Append($@" WHERE {nameof(UserModels.CorporateUser.UserId)} = @{nameof(UserModels.CorporateUser.UserId)}");
                            userQuery.AppendLine($@" END ");
                            userQuery.AppendLine($@"BEGIN ");
                            userQuery.Append($@"UPDATE {DatabaseTables.Users} SET ");
                            userQuery.Append($@"{nameof(UserModels.CorporateUser.ProfileImage)} = @{nameof(UserModels.CorporateUser.ProfileImage)}, ");
                            userQuery.Append($@"{nameof(UserModels.CorporateUser.PostalCode)} = @{nameof(UserModels.CorporateUser.PostalCode)}");
                            userQuery.AppendLine($@" WHERE {nameof(UserModels.User.Id)} = @{nameof(UserModels.User.Id)}");
                            userQuery.Append($@" END");

                            using (SqlCommand cmd = new SqlCommand(userQuery.ToString(), con, trans))
                            {
                                cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.CvrNumber)}", user.CvrNumber);
                                cmd.Parameters.AddWithValue($"@{nameof(UserModels.User.Id)}", user.Id);
                                cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.PostalCode)}", user.PostalCode);
                                cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.ProfileImage)}", user.ProfileImage);
                                cmd.Parameters.AddWithValue($"@{nameof(UserModels.CorporateUser.UserId)}", user.UserId);

                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            log.Error("Something went wrong when trying to update corporated user", ex);
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }

            public static bool ChangePassword(string username, string password)
            {
                using (SqlConnection con = OpenNewConnection())
                {
                    using (SqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder query = new StringBuilder();
                            query.Append($"ALTER LOGIN {username} WITH PASSWORD = '{password}'");

                            using (SqlCommand cmd = new SqlCommand(query.ToString(), con, trans))
                            {
                                cmd.ExecuteNonQuery();
                            }
                            trans.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            log.Error("Something went wrong when trying to change password", ex);
                            return false;
                        }
                    }
                }
            }
            #endregion

            #region Internal
            private static int GetUserId(string username, SqlConnection con, SqlTransaction transaction)
            {
                int userId = 0;
                string query = $@"SELECT {nameof(UserModels.User.Id)} FROM {DatabaseTables.Users} WHERE {nameof(UserModels.User.UserName)} = @UserName";

                using (SqlCommand cmd = new SqlCommand(query, con, transaction))
                {
                    cmd.Parameters.AddWithValue("@UserName", username);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = (int)reader[nameof(UserModels.User.Id)];
                        }
                    }
                }

                return userId;
            }
            #endregion
        }
    }
}
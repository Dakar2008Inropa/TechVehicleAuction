using Microsoft.Data.SqlClient;
using System.Text;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static class Base
        {
            #region Create
            public static int InsertIntoBase(SqlConnection con)
            {
                int baseId;

                StringBuilder Basequery = new StringBuilder();
                Basequery.Append($@"INSERT INTO {DatabaseTables.Base} ");
                Basequery.Append($@"({nameof(Models.Base.CreatedAt)},");
                Basequery.Append($@"{nameof(Models.Base.Status)})");
                Basequery.Append($@" VALUES ");
                Basequery.Append($@"(@{nameof(Models.Base.CreatedAt)},");
                Basequery.Append($@"@{nameof(Models.Base.Status)}); ");
                Basequery.Append($@"SELECT SCOPE_IDENTITY();");

                using (SqlCommand cmd = new SqlCommand(Basequery.ToString(), con))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(Models.Base.CreatedAt)}", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Status)}", (int)BaseStatus.Active);

                    baseId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                return baseId;
            }

            public static int InsertIntoBase(SqlConnection con, SqlTransaction trans)
            {
                int baseId;

                StringBuilder Basequery = new StringBuilder();
                Basequery.Append($@"INSERT INTO {DatabaseTables.Base} ");
                Basequery.Append($@"({nameof(Models.Base.CreatedAt)},");
                Basequery.Append($@"{nameof(Models.Base.Status)})");
                Basequery.Append($@" VALUES ");
                Basequery.Append($@"(@{nameof(Models.Base.CreatedAt)},");
                Basequery.Append($@"@{nameof(Models.Base.Status)}); ");
                Basequery.Append($@"SELECT SCOPE_IDENTITY();");

                using (SqlCommand cmd = new SqlCommand(Basequery.ToString(), con, trans))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(Models.Base.CreatedAt)}", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Status)}", (int)BaseStatus.Active);

                    baseId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                return baseId;
            }
            #endregion

            #region Read

            public static Models.Base GetBase(int baseId, SqlConnection con)
            {
                StringBuilder query = new StringBuilder();
                query.Append($@"SELECT * FROM {DatabaseTables.Base} WHERE {nameof(Models.Base.Id)} = @{nameof(Models.Base.Id)}");

                using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Id)}", baseId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Models.Base modelBase = null;
                        if (reader.Read())
                        {
                            modelBase!.Id = reader.GetInt32(0);
                            modelBase.CreatedAt = reader.GetDateTime(1);
                            modelBase.UpdatedAt = reader.IsDBNull(2) ? null : reader.GetDateTime(2);
                            modelBase.DeletedAt = reader.IsDBNull(3) ? null : reader.GetDateTime(3);
                            modelBase.Status = (BaseStatus)reader.GetInt32(4);
                        }
                        return modelBase!;
                    }
                }
            }

            public static BaseStatus GetStatus(int baseId, SqlConnection con)
            {
                StringBuilder query = new StringBuilder();
                query.Append($@"SELECT {nameof(BaseStatus)} FROM {DatabaseTables.Base} WHERE {nameof(Models.Base.Id)} = @{nameof(Models.Base.Id)}");

                using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Id)}", baseId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (BaseStatus)reader.GetInt32(0);
                        }
                    }
                }

                return BaseStatus.Active;
            }

            public static DateTime GetCreatedAt(int baseId, SqlConnection con)
            {
                StringBuilder query = new StringBuilder();
                query.Append($@"SELECT {nameof(Models.Base.CreatedAt)} FROM {DatabaseTables.Base} WHERE {nameof(Models.Base.Id)} = @{nameof(Models.Base.Id)}");

                using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Id)}", baseId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetDateTime(0);
                        }
                    }
                }
                return DateTime.UtcNow;
            }

            public static DateTime? GetUpdatedAt(int baseId, SqlConnection con)
            {
                StringBuilder query = new StringBuilder();
                query.Append($@"SELECT {nameof(Models.Base.UpdatedAt)} FROM {DatabaseTables.Base} WHERE {nameof(Models.Base.Id)} = @{nameof(Models.Base.Id)}");

                using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Id)}", baseId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetDateTime(0);
                        }
                    }
                }
                return null;
            }

            public static DateTime? GetDeletedAt(int baseId, SqlConnection con)
            {
                StringBuilder query = new StringBuilder();
                query.Append($@"SELECT {nameof(Models.Base.DeletedAt)} FROM {DatabaseTables.Base} WHERE {nameof(Models.Base.Id)} = @{nameof(Models.Base.Id)}");

                using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Id)}", baseId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return reader.GetDateTime(0);
                        }
                    }
                }
                return null;
            }
            #endregion

            #region Update
            public static bool UpdateBase(int baseId)
            {
                using (SqlConnection con = OpenNewConnection())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append($@"UPDATE {DatabaseTables.Base} SET ");
                    query.Append($@"{nameof(Models.Base.UpdatedAt)} = @{nameof(Models.Base.UpdatedAt)} ");
                    query.Append($@"WHERE {nameof(Models.Base.Id)} = @{nameof(Models.Base.Id)}");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(Models.Base.UpdatedAt)}", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Id)}", baseId);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }

            public static bool UpdateBaseStatus(int baseId, BaseStatus status)
            {
                using (SqlConnection con = OpenNewConnection())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append($@"UPDATE {DatabaseTables.Base} SET ");
                    query.Append($@"{nameof(Models.Base.Status)} = @{nameof(Models.Base.Status)} ");
                    query.Append($@"WHERE {nameof(Models.Base.Id)} = @{nameof(Models.Base.Id)}");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(Models.Base.UpdatedAt)}", DateTime.UtcNow);
                        cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Status)}", (int)status);
                        cmd.Parameters.AddWithValue($"@{nameof(Models.Base.Id)}", baseId);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }

            #endregion
        }
    }
}
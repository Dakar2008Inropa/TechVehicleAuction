using Microsoft.Data.SqlClient;
using System.Text;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static class User
        {
            public static bool CreateUser(string username, string password, string discriminator)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(GetConnectionString(Instance.Settings!)))
                    {
                        con.Open();

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
                        ExecuteNonQuery(con, query.ToString());

                        int baseId = InsertIntoBase(con);

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
        }
    }
}
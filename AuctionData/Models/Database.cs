using Microsoft.Data.SqlClient;
using System.Data;

namespace AuctionData.Models
{
    internal class Database
    {
        private readonly string? connectionString;
        private readonly SqlConnection conn;
        public Database()
        {
            SqlConnectionStringBuilder sb = new();
            sb.Clear();
            sb.DataSource = "";
            sb.InitialCatalog = "";
            sb.UserID = "";
            sb.Password = "";
            connectionString = sb.ToString();
            conn = new SqlConnection(connectionString);
        }

        public void CreateAuction(string VehicleId, double AskingPrice)
        {
            using (SqlCommand cmd = new("CreateAuction", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();

                cmd.Parameters.AddWithValue("@VehicleId", SqlDbType.NVarChar).Value = VehicleId;
                cmd.Parameters.AddWithValue("@SellerId", SqlDbType.NVarChar).Value = User.User.user.Id;
                cmd.Parameters.AddWithValue("@AskingPrice", SqlDbType.Decimal).Value = AskingPrice;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
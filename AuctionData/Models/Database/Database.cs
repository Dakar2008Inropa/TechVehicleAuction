using Microsoft.Data.SqlClient;
using System.Data;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        private readonly SqlConnection conn;


        public static Database Instance { get; } = new Database();

        private DbSettings? Settings { get; set; }


        public Database()
        {
            Settings = DbSettings.LoadSettings();

            conn = new SqlConnection(GetConnectionString(Settings));
        }



        private void CreateAuction(string VehicleId, double AskingPrice)
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

        private string GetConnectionString(DbSettings settings)
        {
            SqlConnectionStringBuilder sb = new();
            sb.Clear();
            sb.DataSource = settings.Hostname;
            sb.InitialCatalog = settings.Database;
            sb.UserID = settings.Username;
            sb.Password = settings.Password;
            return sb.ToString();
        }
    }
}
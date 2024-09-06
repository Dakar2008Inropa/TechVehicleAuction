using Microsoft.Data.SqlClient;
using System.Data;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static class Auction
        {
            public static void CreateAuction(int vehicleId, int userId, double askingPrice)
            {
                using (SqlConnection con = new SqlConnection(Instance.GetConnectionString(Instance.Settings!)))
                {
                    using (SqlCommand cmd = new("CreateAuction", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        cmd.Parameters.AddWithValue("@VehicleId", SqlDbType.NVarChar).Value = vehicleId;
                        cmd.Parameters.AddWithValue("@SellerId", SqlDbType.NVarChar).Value = userId;
                        cmd.Parameters.AddWithValue("@AskingPrice", SqlDbType.Decimal).Value = askingPrice;

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }
    }
}
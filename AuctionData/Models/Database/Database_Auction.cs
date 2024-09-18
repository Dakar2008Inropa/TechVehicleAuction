using Microsoft.Data.SqlClient;
using System.Data;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static class Auction
        {
            public static void CreateAuction(int vehicleId, int userId, double minimumAmount)
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString(Instance.Settings!)))
                {
                    using (SqlCommand cmd = new("CreateAuction", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        cmd.Parameters.AddWithValue("@VehicleId", SqlDbType.NVarChar).Value = vehicleId;
                        cmd.Parameters.AddWithValue("@SellerId", SqlDbType.NVarChar).Value = userId;
                        cmd.Parameters.AddWithValue("@MinimumAmount", SqlDbType.Decimal).Value = minimumAmount;

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }
    }
}
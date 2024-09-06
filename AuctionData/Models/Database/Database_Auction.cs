using Microsoft.Data.SqlClient;
using System.Data;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static class Auction
        {
            public static void CreateAuction(string VehicleId, double AskingPrice)
            {
                using (SqlConnection con = new SqlConnection(Instance.GetConnectionString(Instance.Settings!)))
                {
                    using (SqlCommand cmd = new("CreateAuction", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        cmd.Parameters.AddWithValue("@VehicleId", SqlDbType.NVarChar).Value = VehicleId;
                        cmd.Parameters.AddWithValue("@SellerId", SqlDbType.NVarChar).Value = User.User.user.Id;
                        cmd.Parameters.AddWithValue("@AskingPrice", SqlDbType.Decimal).Value = AskingPrice;

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
        }
    }
}
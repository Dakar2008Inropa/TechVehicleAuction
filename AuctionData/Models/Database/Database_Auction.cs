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
                using (SqlConnection conn = new SqlConnection(GetConnectionString(Instance.Settings!)))
                {
                    using (SqlCommand cmd = new("CreateAuction", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();

                        cmd.Parameters.AddWithValue("@VehicleId", SqlDbType.NVarChar).Value = vehicleId;
                        cmd.Parameters.AddWithValue("@SellerId", SqlDbType.NVarChar).Value = userId;
                        cmd.Parameters.AddWithValue("@MinimumAmount", SqlDbType.Decimal).Value = minimumAmount;

                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }

            public static void GetActiveAuctions()
            {
                List<AuctionModels.Auction> list = new();
                using(SqlConnection conn = new SqlConnection( GetConnectionString(Instance.Settings!)))
                {
                    using (SqlCommand cmd = new("GetAuctions", conn))
                    {
                        cmd.CommandType= CommandType.StoredProcedure;
                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            AuctionModels.Auction auction = new AuctionModels.Auction()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                SellerId = reader.GetInt32(reader.GetOrdinal("SellerId")),
                                MinimumAmount = reader.GetDecimal(reader.GetOrdinal("AskingPrice")),
                                CurrentBid = reader.GetDecimal(reader.GetOrdinal("CurrentBid")),
                                VehicleId = reader.GetInt32(reader.GetOrdinal("VehicleId")),
                                Vehicle = new VehicleModels.Vehicle()
                          
                                

                            };
                            list.Add(auction);
                    }
                    conn.Close();
                }
            }
        }
    }
}
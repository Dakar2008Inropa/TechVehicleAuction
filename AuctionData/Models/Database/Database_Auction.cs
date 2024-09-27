using AuctionData.Models.AuctionModels;
using Microsoft.Data.SqlClient;
using System.Text;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static class Auction
        {
            #region Create
            public static void CreateAuction(SqlConnection con, SqlTransaction trans, AuctionModels.Auction auction, int userId, int vehicleId)
            {
                StringBuilder auctionQuery = new StringBuilder();
                auctionQuery.Append($@"INSERT INTO {DatabaseTables.Auctions} ");
                auctionQuery.Append($@"({nameof(AuctionModels.Auction.VehicleId)},");
                auctionQuery.Append($@"{nameof(AuctionModels.Auction.SellerId)},");
                auctionQuery.Append($@"{nameof(AuctionModels.Auction.BaseId)},");
                auctionQuery.Append($@"{nameof(AuctionModels.Auction.MinimumAmount)},");
                auctionQuery.Append($@"{nameof(AuctionModels.Auction.AuctionStatus)},");
                auctionQuery.Append($@"{nameof(AuctionModels.Auction.EndDate)})");

                auctionQuery.Append($@" VALUES ");

                auctionQuery.Append($@"(@{nameof(AuctionModels.Auction.VehicleId)},");
                auctionQuery.Append($@"@{nameof(AuctionModels.Auction.SellerId)},");
                auctionQuery.Append($@"@{nameof(AuctionModels.Auction.BaseId)},");
                auctionQuery.Append($@"@{nameof(AuctionModels.Auction.MinimumAmount)},");
                auctionQuery.Append($@"@{nameof(AuctionModels.Auction.AuctionStatus)},");
                auctionQuery.Append($@"@{nameof(AuctionModels.Auction.EndDate)});");

                int baseId = Base.InsertIntoBase(con, trans);

                using (SqlCommand cmd = new SqlCommand(auctionQuery.ToString(), con, trans))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(AuctionModels.Auction.VehicleId)}", vehicleId);
                    cmd.Parameters.AddWithValue($"@{nameof(AuctionModels.Auction.SellerId)}", userId);
                    cmd.Parameters.AddWithValue($"@{nameof(AuctionModels.Auction.BaseId)}", baseId);
                    cmd.Parameters.AddWithValue($"@{nameof(AuctionModels.Auction.MinimumAmount)}", auction.MinimumAmount);
                    cmd.Parameters.AddWithValue($"@{nameof(AuctionModels.Auction.AuctionStatus)}", AuctionStatus.Active);
                    cmd.Parameters.AddWithValue($"@{nameof(AuctionModels.Auction.EndDate)}", auction.EndDate);

                    cmd.ExecuteNonQuery();
                }
            }

            public static bool CreateAuctionBid(AuctionModels.AuctionBids auctionBid, int userId, int auctionId)
            {
                using (SqlConnection con = OpenNewConnection())
                {
                    using (SqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            StringBuilder auctionBidQuery = new StringBuilder();
                            auctionBidQuery.Append($@"INSERT INTO {DatabaseTables.AuctionBids} ");
                            auctionBidQuery.Append($@"({nameof(AuctionModels.AuctionBids.AuctionId)},");
                            auctionBidQuery.Append($@"{nameof(AuctionModels.AuctionBids.BidderId)},");
                            auctionBidQuery.Append($@"{nameof(AuctionModels.AuctionBids.BidAmount)})");

                            auctionBidQuery.Append($@" VALUES ");

                            auctionBidQuery.Append($@"(@{nameof(AuctionModels.AuctionBids.AuctionId)},");
                            auctionBidQuery.Append($@"@{nameof(AuctionModels.AuctionBids.BidderId)},");
                            auctionBidQuery.Append($@"@{nameof(AuctionModels.AuctionBids.BidAmount)});");

                            int baseId = Base.InsertIntoBase(con, trans);

                            using (SqlCommand cmd = new SqlCommand(auctionBidQuery.ToString(), con, trans))
                            {
                                cmd.Parameters.AddWithValue($"@{nameof(AuctionModels.AuctionBids.AuctionId)}", auctionId);
                                cmd.Parameters.AddWithValue($"@{nameof(AuctionModels.AuctionBids.BidderId)}", userId);
                                cmd.Parameters.AddWithValue($"@{nameof(AuctionModels.AuctionBids.BidAmount)}", auctionBid.BidAmount);
                                cmd.Parameters.AddWithValue($"@{nameof(AuctionModels.AuctionBids.BaseId)}", baseId);

                                cmd.ExecuteNonQuery();
                            }

                            trans.Commit();
                            return true;
                        }
                        catch (SqlException ex)
                        {
                            Console.WriteLine(ex);
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }
            #endregion

            #region Read
            public static AuctionModels.Auction GetAuction(int auctionId)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        StringBuilder auctionQuery = new StringBuilder();
                        auctionQuery.Append(@"SELECT ");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.Id)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.AuctionStatus)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.MinimumAmount)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.VehicleId)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.EndDate)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.SellerId)},");

                        auctionQuery.Append($@"b.{nameof(Models.Base.Status)},");
                        auctionQuery.Append($@"b.{nameof(Models.Base.CreatedAt)},");
                        auctionQuery.Append($@"b.{nameof(Models.Base.UpdatedAt)},");
                        auctionQuery.Append($@"b.{nameof(Models.Base.DeletedAt)},");

                        auctionQuery.Append($@"bids.{nameof(AuctionModels.AuctionBids.BidAmount)},");
                        auctionQuery.Append($@"bids.{nameof(AuctionModels.AuctionBids.BidderId)},");
                        auctionQuery.Append($@"bids.{nameof(AuctionModels.AuctionBids.BaseId)}");

                        auctionQuery.Append($@" FROM ");

                        auctionQuery.Append($@"{DatabaseTables.Auctions} a ");
                        auctionQuery.Append($@"LEFT JOIN {DatabaseTables.Base} b ON a.{nameof(AuctionModels.Auction.BaseId)} = b.{nameof(Models.Base.Id)} ");
                        auctionQuery.Append($@"LEFT JOIN {DatabaseTables.AuctionBids} bids ON a.{nameof(AuctionModels.Auction.Id)} = bids.{nameof(AuctionModels.AuctionBids.AuctionId)}");

                        auctionQuery.Append($@"WHERE a.{nameof(AuctionModels.Auction.Id)} = @{nameof(AuctionModels.Auction.Id)}");

                        using (SqlCommand cmd = new SqlCommand(auctionQuery.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue(nameof(AuctionModels.Auction.Id), auctionId);

                            AuctionModels.Auction auction = new AuctionModels.Auction();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    auction.Id = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.Id)));
                                    auction.VehicleId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.VehicleId)));
                                    auction.SellerId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.SellerId)));
                                    auction.MinimumAmount = reader.GetDecimal(reader.GetOrdinal(nameof(AuctionModels.Auction.MinimumAmount)));
                                    auction.AuctionStatus = (AuctionStatus)reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.AuctionStatus)));
                                    auction.EndDate = reader.GetDateTime(reader.GetOrdinal(nameof(AuctionModels.Auction.EndDate)));
                                    auction.BaseId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.BaseId)));

                                    auction.Status = (BaseStatus)reader.GetInt32(reader.GetOrdinal(nameof(Models.Base.Status)));
                                    auction.CreatedAt = reader.GetDateTime(reader.GetOrdinal(nameof(Models.Base.CreatedAt)));
                                    auction.UpdatedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.UpdatedAt))) ? null : (DateTime)reader[nameof(Models.Base.UpdatedAt)];
                                    auction.DeletedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.DeletedAt))) ? null : (DateTime)reader[nameof(Models.Base.DeletedAt)];
                                }
                            }

                            auction.Vehicle = Vehicle.GetVehicle(auction.VehicleId, con);
                            auction.Seller = User.GetUser(auction.SellerId, con);
                            auction.Bids = GetAuctionBids(auction.Id, con);

                            return auction;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Could not retrieve auction", ex);
                    return new AuctionModels.Auction();
                }
            }

            public static List<AuctionModels.Auction> GetAuctions()
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        List<AuctionModels.Auction> auctions = new List<AuctionModels.Auction>();

                        StringBuilder auctionQuery = new StringBuilder();
                        auctionQuery.Append(@"SELECT ");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.Id)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.AuctionStatus)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.MinimumAmount)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.EndDate)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.VehicleId)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.SellerId)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.BaseId)},");

                        auctionQuery.Append($@"b.{nameof(Models.Base.Status)},");
                        auctionQuery.Append($@"b.{nameof(Models.Base.CreatedAt)},");
                        auctionQuery.Append($@"b.{nameof(Models.Base.UpdatedAt)},");
                        auctionQuery.Append($@"b.{nameof(Models.Base.DeletedAt)},");

                        auctionQuery.Append($@"bids.{nameof(AuctionModels.AuctionBids.BidAmount)},");
                        auctionQuery.Append($@"bids.{nameof(AuctionModels.AuctionBids.BidderId)},");
                        auctionQuery.Append($@"bids.{nameof(AuctionModels.AuctionBids.BaseId)}");

                        auctionQuery.Append($@" FROM ");

                        auctionQuery.Append($@"{DatabaseTables.Auctions} a ");
                        auctionQuery.Append($@"LEFT JOIN {DatabaseTables.Base} b ON a.{nameof(AuctionModels.Auction.BaseId)} = b.{nameof(Models.Base.Id)} ");
                        auctionQuery.Append($@"LEFT JOIN {DatabaseTables.AuctionBids} bids ON a.{nameof(AuctionModels.Auction.Id)} = bids.{nameof(AuctionModels.AuctionBids.AuctionId)}");

                        using (SqlCommand cmd = new SqlCommand(auctionQuery.ToString(), con))
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    AuctionModels.Auction auction = new AuctionModels.Auction();

                                    auction.Id = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.Id)));
                                    auction.VehicleId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.VehicleId)));
                                    auction.SellerId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.SellerId)));
                                    auction.MinimumAmount = reader.GetDecimal(reader.GetOrdinal(nameof(AuctionModels.Auction.MinimumAmount)));
                                    auction.AuctionStatus = (AuctionStatus)reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.AuctionStatus)));
                                    auction.EndDate = reader.GetDateTime(reader.GetOrdinal(nameof(AuctionModels.Auction.EndDate)));
                                    auction.BaseId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.BaseId)));

                                    auction.Status = (BaseStatus)reader.GetInt32(reader.GetOrdinal(nameof(Models.Base.Status)));
                                    auction.CreatedAt = reader.GetDateTime(reader.GetOrdinal(nameof(Models.Base.CreatedAt)));
                                    auction.UpdatedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.UpdatedAt))) ? null : (DateTime)reader[nameof(Models.Base.UpdatedAt)];
                                    auction.DeletedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.DeletedAt))) ? null : (DateTime)reader[nameof(Models.Base.DeletedAt)];

                                    auction.Vehicle = Vehicle.GetVehicle(auction.VehicleId, con);
                                    auction.Seller = User.GetUser(auction.SellerId, con);
                                    auction.Bids = GetAuctionBids(auction.Id, con);

                                    if (auction.MinimumAmount > 0)
                                        auctions.Add(auction);
                                }
                            }
                        }
                        return auctions;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Could not retrieve auctions", ex);
                    return new List<AuctionModels.Auction>();
                }
            }

            public static List<AuctionModels.Auction> GetAuctions(int userId)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        List<AuctionModels.Auction> auctions = new List<AuctionModels.Auction>();

                        StringBuilder auctionQuery = new StringBuilder();
                        auctionQuery.Append(@"SELECT ");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.Id)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.AuctionStatus)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.MinimumAmount)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.EndDate)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.SellerId)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.VehicleId)},");
                        auctionQuery.Append($@"a.{nameof(AuctionModels.Auction.BaseId)},");

                        auctionQuery.Append($@"b.{nameof(Models.Base.Status)},");
                        auctionQuery.Append($@"b.{nameof(Models.Base.CreatedAt)},");
                        auctionQuery.Append($@"b.{nameof(Models.Base.UpdatedAt)},");
                        auctionQuery.Append($@"b.{nameof(Models.Base.DeletedAt)}");

                        auctionQuery.Append($@" FROM ");

                        auctionQuery.Append($@"{DatabaseTables.Auctions} a ");
                        auctionQuery.Append($@"LEFT JOIN {DatabaseTables.Base} b ON a.{nameof(AuctionModels.Auction.BaseId)} = b.{nameof(Models.Base.Id)} ");

                        auctionQuery.Append($@"WHERE a.{nameof(AuctionModels.Auction.SellerId)} = @{nameof(AuctionModels.Auction.SellerId)}");

                        using (SqlCommand cmd = new SqlCommand(auctionQuery.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue(nameof(AuctionModels.Auction.SellerId), userId);


                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    AuctionModels.Auction auction = new AuctionModels.Auction();

                                    auction.Id = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.Id)));
                                    auction.VehicleId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.VehicleId)));
                                    auction.SellerId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.SellerId)));
                                    auction.MinimumAmount = reader.GetDecimal(reader.GetOrdinal(nameof(AuctionModels.Auction.MinimumAmount)));
                                    auction.AuctionStatus = (AuctionStatus)reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.AuctionStatus)));
                                    auction.EndDate = reader.GetDateTime(reader.GetOrdinal(nameof(AuctionModels.Auction.EndDate)));
                                    auction.BaseId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.BaseId)));

                                    auction.Status = (BaseStatus)reader.GetInt32(reader.GetOrdinal(nameof(Models.Base.Status)));
                                    auction.CreatedAt = reader.GetDateTime(reader.GetOrdinal(nameof(Models.Base.CreatedAt)));
                                    auction.UpdatedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.UpdatedAt))) ? null : (DateTime)reader[nameof(Models.Base.UpdatedAt)];
                                    auction.DeletedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.DeletedAt))) ? null : (DateTime)reader[nameof(Models.Base.DeletedAt)];

                                    auction.Vehicle = Vehicle.GetVehicle(auction.VehicleId, con);
                                    auction.Seller = User.GetUser(auction.SellerId, con);
                                    auction.Bids = GetAuctionBids(auction.Id, con);

                                    if (auction.MinimumAmount > 0)
                                        auctions.Add(auction);
                                }
                            }
                        }
                        return auctions;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("could not retrieve auctions", ex);
                    return new List<AuctionModels.Auction>();
                }
            }

            private static List<AuctionModels.AuctionBids> GetAuctionBids(int auctionId, SqlConnection con)
            {
                try
                {
                    StringBuilder bidQuery = new StringBuilder();

                    bidQuery.Append(@"SELECT ");
                    bidQuery.Append($@"bid.{nameof(AuctionModels.AuctionBids.Id)},");
                    bidQuery.Append($@"bid.{nameof(AuctionModels.AuctionBids.AuctionId)},");
                    bidQuery.Append($@"bid.{nameof(AuctionModels.AuctionBids.BidAmount)},");
                    bidQuery.Append($@"bid.{nameof(AuctionModels.AuctionBids.BidderId)},");
                    bidQuery.Append($@"bid.{nameof(AuctionModels.AuctionBids.BaseId)},");

                    bidQuery.Append($@"b.{nameof(Models.Base.CreatedAt)},");
                    bidQuery.Append($@"b.{nameof(Models.Base.UpdatedAt)},");
                    bidQuery.Append($@"b.{nameof(Models.Base.DeletedAt)},");
                    bidQuery.Append($@"b.{nameof(Models.Base.Status)}");

                    bidQuery.Append(" FROM ");

                    bidQuery.Append($"{DatabaseTables.AuctionBids} bid ");
                    bidQuery.Append($"LEFT JOIN {DatabaseTables.Base} b ON bid.{nameof(AuctionModels.AuctionBids.BaseId)} = b.{nameof(Models.Base.Id)} ");
                    bidQuery.Append($"WHERE bid.{nameof(AuctionModels.AuctionBids.AuctionId)} = @{nameof(AuctionModels.AuctionBids.AuctionId)}");

                    using (SqlCommand cmd = new SqlCommand(bidQuery.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.AuctionBids.AuctionId), auctionId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            List<AuctionModels.AuctionBids> bids = new List<AuctionBids>();

                            while (reader.Read())
                            {
                                AuctionModels.AuctionBids bid = new AuctionModels.AuctionBids
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.Id))),
                                    AuctionId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.AuctionId))),
                                    BidAmount = reader.GetDecimal(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.BidAmount))),
                                    BidderId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.BidderId))),
                                    BaseId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.BaseId))),
                                    CreatedAt = reader.GetDateTime(reader.GetOrdinal(nameof(Models.Base.CreatedAt))),
                                    UpdatedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.UpdatedAt))) ? null : (DateTime)reader[nameof(Models.Base.UpdatedAt)],
                                    DeletedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.DeletedAt))) ? null : (DateTime)reader[nameof(Models.Base.DeletedAt)],
                                    Status = (BaseStatus)reader.GetInt32(reader.GetOrdinal(nameof(Models.Base.Status)))
                                };

                                bids.Add(bid);
                            }
                            return bids;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Could not retrieve auction bids", ex);
                    return new List<AuctionBids>();
                }
            }
            #endregion
        }
    }
}
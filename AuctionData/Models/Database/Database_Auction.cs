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
            public static bool CreateAuction(AuctionModels.Auction auction, int userId, int vehicleId)
            {
                using (SqlConnection con = OpenNewConnection())
                {

                    int baseId = Base.InsertIntoBase(con);

                    StringBuilder auctionQuery = new StringBuilder();
                    auctionQuery.Append($@"INSERT INTO {DatabaseTables.Auctions} ");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.VehicleId)},");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.SellerId)},");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.BaseId)},");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.MinimumAmount)},");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.AuctionStatus)},");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.EndDate)})");

                    auctionQuery.Append($@" VALUES ");

                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.VehicleId)},");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.SellerId)},");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.BaseId)},");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.MinimumAmount)},");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.AuctionStatus)},");
                    auctionQuery.Append($@"({nameof(AuctionModels.Auction.EndDate)});");

                    using (SqlCommand cmd = new SqlCommand(auctionQuery.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.Auction.VehicleId), vehicleId);
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.Auction.SellerId), userId);
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.Auction.BaseId), baseId);
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.Auction.MinimumAmount), auction.MinimumAmount);
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.Auction.AuctionStatus), AuctionStatus.Active);
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.Auction.EndDate), auction.EndDate);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }

            public static bool CreateAuctionBid(AuctionModels.AuctionBids auctionBid, int userId, int auctionId)
            {
                using (SqlConnection con = OpenNewConnection())
                {
                    int baseId = Base.InsertIntoBase(con);

                    StringBuilder auctionBidQuery = new StringBuilder();
                    auctionBidQuery.Append($@"INSERT INTO {DatabaseTables.AuctionBids} ");
                    auctionBidQuery.Append($@"({nameof(AuctionModels.AuctionBids.AuctionId)},");
                    auctionBidQuery.Append($@"({nameof(AuctionModels.AuctionBids.BidderId)},");
                    auctionBidQuery.Append($@"({nameof(AuctionModels.AuctionBids.BidAmount)})");

                    auctionBidQuery.Append($@" VALUES ");

                    auctionBidQuery.Append($@"({nameof(AuctionModels.AuctionBids.AuctionId)},");
                    auctionBidQuery.Append($@"({nameof(AuctionModels.AuctionBids.BidderId)},");
                    auctionBidQuery.Append($@"({nameof(AuctionModels.AuctionBids.BidAmount)});");

                    using (SqlCommand cmd = new SqlCommand(auctionBidQuery.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.AuctionBids.AuctionId), auctionId);
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.AuctionBids.BidderId), userId);
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.AuctionBids.BidAmount), auctionBid.BidAmount);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            #endregion

            #region Read
            public static AuctionModels.Auction GetAuction(int auctionId)
            {
                using (SqlConnection con = OpenNewConnection())
                {
                    StringBuilder auctionQuery = new StringBuilder();
                    auctionQuery.Append($@"SELECT * FROM {DatabaseTables.Auctions} ");
                    auctionQuery.Append($@"WHERE {nameof(AuctionModels.Auction.Id)} = @{nameof(AuctionModels.Auction.Id)}");

                    using (SqlCommand cmd = new SqlCommand(auctionQuery.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.Auction.Id), auctionId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new AuctionModels.Auction
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.Id))),
                                    VehicleId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.VehicleId))),
                                    SellerId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.SellerId))),
                                    MinimumAmount = reader.GetDecimal(reader.GetOrdinal(nameof(AuctionModels.Auction.MinimumAmount))),
                                    AuctionStatus = (AuctionStatus)reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.AuctionStatus))),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal(nameof(AuctionModels.Auction.EndDate))),
                                    BaseId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.BaseId)))
                                };
                            }
                        }
                    }
                }

                return null;
            }

            public static List<AuctionModels.Auction> GetAuctions()
            {
                List<AuctionModels.Auction> auctions = new List<AuctionModels.Auction>();

                using (SqlConnection con = OpenNewConnection())
                {
                    StringBuilder auctionQuery = new StringBuilder();
                    auctionQuery.Append($@"SELECT * FROM {DatabaseTables.Auctions}");

                    using (SqlCommand cmd = new SqlCommand(auctionQuery.ToString(), con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                auctions.Add(new AuctionModels.Auction
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.Id))),
                                    VehicleId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.VehicleId))),
                                    SellerId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.SellerId))),
                                    MinimumAmount = reader.GetDecimal(reader.GetOrdinal(nameof(AuctionModels.Auction.MinimumAmount))),
                                    AuctionStatus = (AuctionStatus)reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.AuctionStatus))),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal(nameof(AuctionModels.Auction.EndDate))),
                                    BaseId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.BaseId)))
                                });
                            }
                        }
                    }
                }

                return auctions;
            }

            public static List<AuctionModels.Auction> GetAuctions(int userId)
            {
                List<AuctionModels.Auction> auctions = new List<AuctionModels.Auction>();

                using (SqlConnection con = OpenNewConnection())
                {
                    StringBuilder auctionQuery = new StringBuilder();
                    auctionQuery.Append($@"SELECT * FROM {DatabaseTables.Auctions} ");
                    auctionQuery.Append($@"WHERE {nameof(AuctionModels.Auction.SellerId)} = @{nameof(AuctionModels.Auction.SellerId)}");

                    using (SqlCommand cmd = new SqlCommand(auctionQuery.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.Auction.SellerId), userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                auctions.Add(new AuctionModels.Auction
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.Id))),
                                    VehicleId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.VehicleId))),
                                    SellerId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.SellerId))),
                                    MinimumAmount = reader.GetDecimal(reader.GetOrdinal(nameof(AuctionModels.Auction.MinimumAmount))),
                                    AuctionStatus = (AuctionStatus)reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.AuctionStatus))),
                                    EndDate = reader.GetDateTime(reader.GetOrdinal(nameof(AuctionModels.Auction.EndDate))),
                                    BaseId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.Auction.BaseId)))
                                });
                            }
                        }
                    }
                }

                return auctions;
            }

            public static List<AuctionModels.AuctionBids> GetAuctionBids(int auctionId)
            {
                List<AuctionModels.AuctionBids> auctionBids = new List<AuctionModels.AuctionBids>();

                using (SqlConnection con = OpenNewConnection())
                {
                    StringBuilder auctionBidQuery = new StringBuilder();
                    auctionBidQuery.Append($@"SELECT * FROM {DatabaseTables.AuctionBids} ");
                    auctionBidQuery.Append($@"WHERE {nameof(AuctionModels.AuctionBids.AuctionId)} = @{nameof(AuctionModels.AuctionBids.AuctionId)}");

                    using (SqlCommand cmd = new SqlCommand(auctionBidQuery.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.AuctionBids.AuctionId), auctionId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                auctionBids.Add(new AuctionModels.AuctionBids
                                {
                                    AuctionId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.AuctionId))),
                                    BidderId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.BidderId))),
                                    BidAmount = reader.GetDecimal(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.BidAmount))),
                                    BaseId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.BaseId)))
                                });
                            }
                        }
                    }
                }

                return auctionBids;
            }

            public static AuctionModels.AuctionBids GetCurrentHighestBid(int auctionId)
            {
                AuctionModels.AuctionBids auctionBid = new AuctionBids();

                using (SqlConnection con = OpenNewConnection())
                {
                    StringBuilder auctionBidQuery = new StringBuilder();
                    auctionBidQuery.Append($@"SELECT TOP 1 * FROM {DatabaseTables.AuctionBids} ");
                    auctionBidQuery.Append($@"WHERE {nameof(AuctionModels.AuctionBids.AuctionId)} = @{nameof(AuctionModels.AuctionBids.AuctionId)} ");
                    auctionBidQuery.Append($@"ORDER BY {nameof(AuctionModels.AuctionBids.BidAmount)} DESC");

                    using (SqlCommand cmd = new SqlCommand(auctionBidQuery.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue(nameof(AuctionModels.AuctionBids.AuctionId), auctionId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                auctionBid.AuctionId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.AuctionId)));
                                auctionBid.BidderId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.BidderId)));
                                auctionBid.BidAmount = reader.GetDecimal(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.BidAmount)));
                                auctionBid.BaseId = reader.GetInt32(reader.GetOrdinal(nameof(AuctionModels.AuctionBids.BaseId)));
                                auctionBid.Status = Base.GetStatus(auctionBid.BaseId, con);
                                auctionBid.CreatedAt = Base.GetCreatedAt(auctionBid.BaseId, con);
                                auctionBid.UpdatedAt = Base.GetUpdatedAt(auctionBid.BaseId, con);
                                auctionBid.DeletedAt = Base.GetDeletedAt(auctionBid.BaseId, con);
                            }
                        }
                    }
                }
                return auctionBid;
            }
            #endregion
        }
    }
}
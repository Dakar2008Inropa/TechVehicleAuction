using AuctionData.Models.UserModels;

namespace AuctionData.Models.AuctionModels
{
    public class AuctionBids : Base
    {
        public int AuctionId { get; set; }
        public int BidderId { get; set; }
        public decimal BidAmount { get; set; }
        public int BaseId { get; set; }


        public AuctionBids()
        {
        }
        public AuctionBids(Auction? auction, User? bidder, ulong bidAmount)
        {
            BidAmount = bidAmount;
        }
    }
}
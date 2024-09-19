using AuctionData.Models.UserModels;

namespace AuctionData.Models.AuctionModels
{
    public class AuctionBids : Base
    {
        public Auction? Auction { get; set; }
        public int AuctionId { get; set; }
        public User? Bidder { get; set; }
        public int BidderId { get; set; }
        public ulong BidAmount { get; set; }
        public int BaseId { get; set; }
        public Base? Base { get; set; }


        public AuctionBids()
        {
        }
        public AuctionBids(Auction? auction, User? bidder, ulong bidAmount)
        {
            Auction = auction;
            Bidder = bidder;
            BidAmount = bidAmount;
        }


        public AuctionBids PlaceBid(Auction? auction, User? bidder, ulong bidAmount)
        {
            return new AuctionBids(auction, bidder, bidAmount);
        }
        public void FindBidById()
        {

        }
    }
}
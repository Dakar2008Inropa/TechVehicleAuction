using AuctionData.Models.UserModels;
using AuctionData.Models.VehicleModels;

namespace AuctionData.Models.AuctionModels;
public class Auction : Base
{
    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }
    public int SellerId { get; set; }
    public User Seller { get; set; }
    public decimal MinimumAmount { get; set; }

    public AuctionStatus AuctionStatus { get; set; }
    public DateTime EndDate { get; set; }
    public List<AuctionBids> Bids { get; set; }
    public int BaseId { get; set; }


    public Auction()
    {
    }

    public Auction(Vehicle? vehicle, User? seller, ulong minAmount)
    {
        MinimumAmount = minAmount;
    }
}
using AuctionData.Models.UserModels;
using AuctionData.Models.VehicleModels;

namespace AuctionData.Models.AuctionModels;
public class Auction : Base
{
    public int VehicleId { get; set; }
    public int SellerId { get; set; }
    public decimal MinimumAmount { get; set; }

    public AuctionStatus AuctionStatus { get; set; }
    public DateTime EndDate { get; set; }
    public int BaseId { get; set; }


    public Auction()
    {
    }

    public Auction(Vehicle? vehicle, User? seller, ulong minAmount)
    {
        MinimumAmount = minAmount;
    }
}
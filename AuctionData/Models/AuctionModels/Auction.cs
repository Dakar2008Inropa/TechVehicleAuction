namespace AuctionData.Models.AuctionModels;
public class Auction : Base
{
    public Vehicle.Vehicle? Vehicle { get; set; }
    public User.User? Seller { get; set; }
    public ulong MinimumAmount { get; set; }

    public Auction()
    {
    }
    public Auction(Vehicle.Vehicle? vehicle, User.User? seller, ulong minAmount)
    {
        Vehicle = vehicle;
        Seller = seller;
        MinimumAmount = minAmount;
    }
    public Auction SetForSale(Vehicle.Vehicle? vehicle, User.User? seller, ulong minAmount)
    {
        return new Auction(vehicle, seller, minAmount);
    }
    public void RecieveBid(ulong bidAmount)
    {
    }
    public void AcceptBid()
    {
    }
    public void FindAuctionById()
    {

    }
}
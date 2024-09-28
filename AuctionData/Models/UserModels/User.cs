using AuctionData.Models.Interfaces;

namespace AuctionData.Models.UserModels;

public abstract class User : Base, IUser
{
    public string? UserName { get; set; }
    public string? PostalCode { get; set; }
    public string? Discriminator { get; set; }
    public int BaseId { get; set; }

    public User()
    {
    }
    public override string ToString()
    {
        return $"ID: {Id} " +
            $"Username: {UserName} " +
            $"Postal code: {PostalCode}";
    }
}
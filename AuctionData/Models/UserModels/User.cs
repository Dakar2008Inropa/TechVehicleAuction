using AuctionData.Models.Interfaces;

namespace AuctionData.Models.UserModels;

public abstract class User : Base, IUser
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public ushort PostalCode { get; set; }
    public string? Discriminator { get; set; }

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
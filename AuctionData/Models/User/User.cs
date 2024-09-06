using AuctionData.Models.Interfaces;

namespace AuctionData.Models.User;

public class User : Base, IUser
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public ushort PostalCode { get; set; }

    public static User user = new User();
    public User()
    {
        //database...
    }
    public override string ToString()
    {
        return $"ID: {Id} " +
            $"Username: {UserName} " +
            $"Postal code: {PostalCode}";
    }
}
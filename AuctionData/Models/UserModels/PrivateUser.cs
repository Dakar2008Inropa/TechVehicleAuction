namespace AuctionData.Models.UserModels;

public class PrivateUser : User
{
    public string? CPRNumber { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public PrivateUser()
    {

    }
}
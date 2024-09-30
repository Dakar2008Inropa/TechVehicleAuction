namespace AuctionData.Models.UserModels;

public class CorporateUser : User
{
    public decimal Credit { get; set; }
    public string? CvrNumber { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public CorporateUser()
    {

    }

    public override string ToString()
    {
        return $"Credit: {Credit} " +
            $"CVRNumber: {CvrNumber}";
    }
}
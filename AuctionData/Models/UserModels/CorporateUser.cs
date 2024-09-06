using AuctionData.Models.UserModels.ValidationAttributes;

namespace AuctionData.Models.UserModels;

public class CorporateUser : User
{
    public long Credit { get; set; }
    public string? CvrNumber { get; set; }
    public CorporateUser(long credit, string cvrNumber)
    {
        Credit = credit;
        if (CvrNumberValidation.ValidateCvrNumber(cvrNumber))
            CvrNumber = cvrNumber;

        //db connection...
    }
    public override string ToString()
    {
        return $"Credit: {Credit} " +
            $"CVRNumber: {CvrNumber}";
    }
}
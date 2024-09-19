using AuctionData.Models.UserModels.Generators;
using AuctionData.Models.UserModels.ValidationAttributes;

namespace AuctionData.Models.UserModels;

public class PrivateUser : User
{
    public string? CPRNumber { get; private set; }
    public int UserId { get; set; }
    public User? User { get; set; }

    public PrivateUser()
    {

    }

    public PrivateUser(string cprNumber)
    {
        CPRNumber = string.Empty;
        if (CprNumberValidation.IsValidCpr(cprNumber))
        {
            CPRNumber = cprNumber;
        }
        else
        {
            var cpr = CprGenerator.GenerateCPR();
            if (CprNumberValidation.IsValidCpr(cpr))
            {
                CPRNumber = cpr;
            }
        }
    }
}
using AuctionData.Models.UserModels.Generators;
using AuctionData.Models.UserModels.ValidationAttributes;

namespace AuctionData.Models.UserModels;

public class PrivateUser : User
{
    public string? CPRNumber { get; private set; }

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
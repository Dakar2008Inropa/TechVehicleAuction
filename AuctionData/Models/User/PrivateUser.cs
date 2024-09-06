using AuctionData.Models.User.Generators;
using AuctionData.Models.User.ValidationAttributes;

namespace AuctionData.Models.User;

public class PrivateUser
{
    public string CPRNumber { get; private set; }

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

        //DB U11
    }

}

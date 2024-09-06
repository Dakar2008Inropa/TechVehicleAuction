namespace AuctionData.Models.UserModels.ValidationAttributes;

public static class CvrNumberValidation
{
    public static bool ValidateCvrNumber(string CvrNumber)
    {
        if (string.IsNullOrEmpty(CvrNumber))
        {
            return false;
        }

        if (CvrNumber.Length != 8)
        {
            return false;
        }

        if (!ulong.TryParse(CvrNumber, out _))
        {
            return false;
        }

        if (CvrNumber[0] == '0')
        {
            return false;
        }

        return true;
    }
}

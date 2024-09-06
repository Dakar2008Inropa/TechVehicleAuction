namespace AuctionData.Models.User.ValidationAttributes;

public static class CprNumberValidation
{
    public static bool IsValidCpr(string cpr)
    {
        cpr = cpr.Replace("-", "");

        if (cpr.Length != 10 || !long.TryParse(cpr, out _))
        {
            return false;
        }

        string datePart = cpr.Substring(0, 6);

        int sequenceNumber = int.Parse(cpr.Substring(6, 4));
        int century;
        int year = int.Parse(datePart.Substring(4, 2));

        if (sequenceNumber >= 0000 && sequenceNumber <= 4999)
        {
            century = 1900;

        }

        else if (sequenceNumber >= 5000 && sequenceNumber <= 9999)
        {
            century = 2000;
        }

        else
        {
            return false;
        }

        int fullYear = century + year;

        if (fullYear >= 2007)
        {
            return IsValidMod11(cpr);
        }

        return true;
    }
    private static bool IsValidMod11(string cpr)
    {
        int[] weights = [4, 3, 2, 7, 6, 5, 4, 3, 2];
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (cpr[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = (remainder == 0) ? 0 : 11 - remainder;

        return checkDigit == (cpr[9] - '0');
    }

}
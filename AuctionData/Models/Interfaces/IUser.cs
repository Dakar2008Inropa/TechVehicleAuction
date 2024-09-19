namespace AuctionData.Models.Interfaces;

public interface IUser
{
    string? UserName { get; set; }
    ushort PostalCode { get; set; }
    string ToString();
}
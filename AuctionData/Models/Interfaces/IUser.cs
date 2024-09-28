namespace AuctionData.Models.Interfaces;

public interface IUser
{
    string? UserName { get; set; }
    string? PostalCode { get; set; }
    string ToString();
}
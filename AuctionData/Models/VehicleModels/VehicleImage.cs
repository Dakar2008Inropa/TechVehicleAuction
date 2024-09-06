namespace AuctionData.Models.VehicleModels
{
    public class VehicleImage : Base
    {
        public string? Image { get; set; }
        public string? Description { get; set; }
        public Vehicle? Vehicle { get; set; }
    }
}
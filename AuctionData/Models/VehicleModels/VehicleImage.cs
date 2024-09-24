namespace AuctionData.Models.VehicleModels
{
    public class VehicleImage : Base
    {
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public Vehicle? Vehicle { get; set; }
        public int VehicleId { get; set; }
        public int BaseId { get; set; }
    }
}
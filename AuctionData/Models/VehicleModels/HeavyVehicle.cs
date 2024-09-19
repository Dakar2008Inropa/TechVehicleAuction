namespace AuctionData.Models.VehicleModels
{
    public abstract class HeavyVehicle : Vehicle
    {
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public override string ToString()
        {
            return $"HeavyVehicle: {base.ToString()}, Height: {Height}, Weight: {Weight}, Length: {Length}";
        }

        public new double GetEngineSize()
        {
            if (EngineSize >= 4.2 && EngineSize <= 15.0)
                return EngineSize;

            throw new ArgumentOutOfRangeException(EngineSize.ToString(), "Engine size must be between 4.2 and 15.0 Litre");
        }
    }
}
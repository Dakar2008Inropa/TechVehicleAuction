namespace AuctionData.Models.VehicleModels
{
    public abstract class PassengerCar : Vehicle
    {
        public double TrunkWidth { get; set; }
        public double TrunkHeight { get; set; }
        public double TrunkLength { get; set; }

        public int SeatCapacity { get; set; }
        public bool RequireCommercialLicense { get; set; }
        public double TrunkDimensions { get; set; }
        public int VehicleId { get; set; }


        protected PassengerCar()
        {
            LicenseType = GetLicenseType();
        }

        public new LicenseType GetLicenseType()
        {
            if (RequireCommercialLicense)
                return LicenseType.BE;

            return LicenseType.B;
        }

        public new double GetEngineSize()
        {
            if (EngineSize >= 0.7 && EngineSize <= 10.0)
                return EngineSize;

            throw new ArgumentOutOfRangeException(EngineSize.ToString(), "Engine size must be between 0.7 and 10.0 Litre");
        }

        public static double GetTrunkCapacity(double trunkWidth, double trunkHeight, double trunkLength)
        {
            double volumeCubicCm = trunkWidth * trunkHeight * trunkLength;

            double capacityLiters = volumeCubicCm / 1000;

            return capacityLiters;
        }

        public override string ToString()
        {
            return $"PassengerCar: {base.ToString()}, Trunk Capacity: {TrunkDimensions}, Seats: {SeatCapacity}";
        }
    }
}
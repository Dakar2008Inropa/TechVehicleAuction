namespace AuctionData.Models.Vehicle
{
    public class Bus : HeavyVehicle
    {
        public int SeatingCapacity { get; set; }
        public int SleepingCapacity { get; set; }
        public bool Toilet { get; set; }

        protected Bus()
        {
            LicenseType = GetLicenseType();
            EngineSize = GetEngineSize();
        }

        public new LicenseType GetLicenseType()
        {
            if (Towinghitch)
                return LicenseType.DE;

            return LicenseType.D;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Seating Capacity: {SeatingCapacity}, Sleeping Capacity: {SleepingCapacity}, Toilets: {Toilet}";
        }
    }
}
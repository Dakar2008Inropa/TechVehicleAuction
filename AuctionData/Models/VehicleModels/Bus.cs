namespace AuctionData.Models.VehicleModels
{
    public class Bus : HeavyVehicle
    {
        public int SeatingCapacity { get; set; }
        public int SleepingCapacity { get; set; }
        public bool Toilet { get; set; }
        public int HeavyVehicleId { get; set; }
        public HeavyVehicle? HeavyVehicle { get; set; }

        public Bus()
        {
            LicenseType = GetLicenseType();
            EnergyClass = (EnergyClass)GetEnergyClass();
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
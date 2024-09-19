namespace AuctionData.Models.VehicleModels
{
    public class Truck : HeavyVehicle
    {
        public int LoadCapacity { get; set; }
        public int HeavyVehicleId { get; set; }
        public HeavyVehicle? HeavyVehicle { get; set; }

        public Truck()
        {
            LicenseType = GetLicenseType();
        }

        public new LicenseType GetLicenseType()
        {
            if (Towinghitch)
                return LicenseType.C;

            return LicenseType.CE;
        }

        public override string ToString()
        {
            return $"{base.ToString()} Load Capacity: {LoadCapacity}";
        }
    }
}
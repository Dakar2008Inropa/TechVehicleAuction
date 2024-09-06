namespace AuctionData.Models.Vehicle
{
    public class Truck : HeavyVehicle
    {
        public int LoadCapacity { get; set; }

        protected Truck()
        {
            LicenseType = GetLicenseType();
            EngineSize = GetEngineSize();
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
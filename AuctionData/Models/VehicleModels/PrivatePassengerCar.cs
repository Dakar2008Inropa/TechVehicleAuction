namespace AuctionData.Models.VehicleModels
{
    public class PrivatePassengerCar : PassengerCar
    {
        public bool IsofixMounts { get; set; }
        public int PassengerCarId { get; set; }
        public PassengerCar? PassengerCar { get; set; }

        public PrivatePassengerCar()
        {
            LicenseType = GetLicenseType();
            EnergyClass = (EnergyClass)GetEnergyClass();
        }

        public override string ToString()
        {
            return $"PrivatePassengerCar: {base.ToString()}, IsofixMounts: {IsofixMounts}";
        }
    }
}
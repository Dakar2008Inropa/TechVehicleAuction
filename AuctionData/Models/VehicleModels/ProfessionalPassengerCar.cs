namespace AuctionData.Models.VehicleModels
{
    public class ProfessionalPassengerCar : PassengerCar
    {
        public bool RollCage { get; set; }
        public bool FireExtinguisher { get; set; }
        public bool RacingSeat { get; set; }
        public bool RacingHarness { get; set; }
        public int LoadCapacity { get; set; }
        public int PassengerCarId { get; set; }
        public PassengerCar? PassengerCar { get; set; }

        public ProfessionalPassengerCar()
        {
            LicenseType = GetLicenseType();
            EnergyClass = (EnergyClass)GetEnergyClass();
        }

        public override string ToString()
        {
            return $"ProfessionalPassengerCar: {base.ToString()}, RollCage: {RollCage}, FireExtinguisher: {FireExtinguisher}, RacingSeat: {RacingSeat}, RacingHarness: {RacingHarness}, LoadCapacity: {LoadCapacity}";
        }
    }
}
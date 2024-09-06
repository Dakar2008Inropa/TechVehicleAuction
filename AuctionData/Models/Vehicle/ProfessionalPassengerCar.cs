namespace AuctionData.Models.Vehicle
{
    public class ProfessionalPassengerCar : PassengerCar
    {
        public bool RollCage { get; set; }
        public bool FireExtinguisher { get; set; }
        public bool RacingSeat { get; set; }
        public bool RacingHarness { get; set; }
        public int LoadCapacity { get; set; }

        public override string ToString()
        {
            return $"ProfessionalPassengerCar: {base.ToString()}, RollCage: {RollCage}, FireExtinguisher: {FireExtinguisher}, RacingSeat: {RacingSeat}, RacingHarness: {RacingHarness}, LoadCapacity: {LoadCapacity}";
        }
    }
}
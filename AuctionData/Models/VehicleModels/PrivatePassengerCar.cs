namespace AuctionData.Models.VehicleModels
{
    public class PrivatePassengerCar : PassengerCar
    {
        public bool IsofixMounts { get; set; }

        public override string ToString()
        {
            return $"PrivatePassengerCar: {base.ToString()}, IsofixMounts: {IsofixMounts}";
        }
    }
}
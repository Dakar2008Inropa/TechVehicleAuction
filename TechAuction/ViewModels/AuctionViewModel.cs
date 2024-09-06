using AuctionData.Models.Vehicle;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TechAuction.ViewModels
{
    public class AuctionViewModel : ViewModelBase
    {
        public ObservableCollection<Vehicle> Vehicles { get; }

        public AuctionViewModel()
        {
            var vehicles = new List<Vehicle>
            {
                new PrivatePassengerCar { Maker = "BMW", Model = "530d", ModelYear = 2017, EngineSize = 3.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "740i", ModelYear = 2012, EngineSize = 12.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "320i", ModelYear = 2013, EngineSize = 2.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "730d", ModelYear = 2014, EngineSize = 3.0}
            };
            Vehicles = new ObservableCollection<Vehicle>(vehicles);
        }
    }
}
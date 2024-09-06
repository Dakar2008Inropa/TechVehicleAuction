using AuctionData.Models.VehicleModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TechAuction.ViewModels
{
    public class AuctionViewModel : ViewModelBase
    {
        public ObservableCollection<Vehicle> YourAuctions { get; }
        public ObservableCollection<Vehicle> Auctions { get; }

        public AuctionViewModel()
        {
            var yourauctions = new List<Vehicle>
            {
                new PrivatePassengerCar { Maker = "BMW", Model = "530d", ModelYear = 2017, EngineSize = 3.0},
                new PrivatePassengerCar { Maker = "Audi", Model = "A4", ModelYear = 2012, EngineSize = 4.6},
                new PrivatePassengerCar { Maker = "Mercedes", Model = "E320", ModelYear = 2013, EngineSize = 3.2},
                new PrivatePassengerCar { Maker = "Honda", Model = "Accord 2.0", ModelYear = 2014, EngineSize = 2.0},
                new ProfessionalPassengerCar { Maker = "Audi", Model = "R8", ModelYear = 2001, EngineSize = 10.0},
                new Truck {Maker = "Scania", Model="R730 V8", ModelYear = 2010, EngineSize = 2.0},
                new Truck {Maker = "Volvo", Model="620", ModelYear = 2010, EngineSize = 2.0},
                new Truck {Maker = "DAF", Model="XF V6", ModelYear = 2010, EngineSize = 2.0},
                new Bus {Maker = "Scania", Model="Longrun V8", ModelYear = 2012, EngineSize = 2.0},
                new Bus {Maker = "Volvo", Model="Busman V6", ModelYear = 2012, EngineSize = 2.0},
                new Bus {Maker = "Tatra", Model="Boring", ModelYear = 2012, EngineSize = 2.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "530d", ModelYear = 2017, EngineSize = 3.0},
                new PrivatePassengerCar { Maker = "Audi", Model = "A4", ModelYear = 2012, EngineSize = 4.6},
                new PrivatePassengerCar { Maker = "Mercedes", Model = "E320", ModelYear = 2013, EngineSize = 3.2},
                new PrivatePassengerCar { Maker = "Honda", Model = "Accord 2.0", ModelYear = 2014, EngineSize = 2.0},
                new ProfessionalPassengerCar { Maker = "Audi", Model = "R8", ModelYear = 2001, EngineSize = 10.0},
                new Truck {Maker = "Scania", Model="R730 V8", ModelYear = 2010, EngineSize = 2.0},
                new Truck {Maker = "Volvo", Model="620", ModelYear = 2010, EngineSize = 2.0},
                new Truck {Maker = "DAF", Model="XF V6", ModelYear = 2010, EngineSize = 2.0},
                new Bus {Maker = "Scania", Model="Longrun V8", ModelYear = 2012, EngineSize = 2.0},
                new Bus {Maker = "Volvo", Model="Busman V6", ModelYear = 2012, EngineSize = 2.0},
                new Bus {Maker = "Tatra", Model="Boring", ModelYear = 2012, EngineSize = 2.0}
            };
            YourAuctions = new ObservableCollection<Vehicle>(yourauctions);

            var auctions = new List<Vehicle>
            {
                new PrivatePassengerCar { Maker = "BMW", Model = "530d", ModelYear = 2017, EngineSize = 3.0},
                new PrivatePassengerCar { Maker = "Audi", Model = "S8", ModelYear = 2012, EngineSize = 4.8},
                new PrivatePassengerCar { Maker = "Mercedes", Model = "G500", ModelYear = 2013, EngineSize = 5.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "M5 Competizion", ModelYear = 2014, EngineSize = 6.0},
                new PrivatePassengerCar { Maker = "Hyundai", Model = "i30", ModelYear = 2017, EngineSize = 1.6},
                new PrivatePassengerCar { Maker = "Volkswagen", Model = "Golf 2 GTI 16v", ModelYear = 2012, EngineSize = 2.0},
                new PrivatePassengerCar { Maker = "Peugeot", Model = "206+", ModelYear = 2013, EngineSize = 1.4},
                new PrivatePassengerCar { Maker = "Alfa Romeo", Model = "156", ModelYear = 2014, EngineSize = 2.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "530d", ModelYear = 2017, EngineSize = 3.0},
                new PrivatePassengerCar { Maker = "Audi", Model = "S8", ModelYear = 2012, EngineSize = 4.8},
                new PrivatePassengerCar { Maker = "Mercedes", Model = "G500", ModelYear = 2013, EngineSize = 5.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "M5 Competizion", ModelYear = 2014, EngineSize = 6.0},
                new PrivatePassengerCar { Maker = "Hyundai", Model = "i30", ModelYear = 2017, EngineSize = 1.6},
                new PrivatePassengerCar { Maker = "Volkswagen", Model = "Golf 2 GTI 16v", ModelYear = 2012, EngineSize = 2.0},
                new PrivatePassengerCar { Maker = "Peugeot", Model = "206+", ModelYear = 2013, EngineSize = 1.4},
                new PrivatePassengerCar { Maker = "Alfa Romeo", Model = "156", ModelYear = 2014, EngineSize = 2.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "530d", ModelYear = 2017, EngineSize = 3.0},
                new PrivatePassengerCar { Maker = "Audi", Model = "S8", ModelYear = 2012, EngineSize = 4.8},
                new PrivatePassengerCar { Maker = "Mercedes", Model = "G500", ModelYear = 2013, EngineSize = 5.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "M5 Competizion", ModelYear = 2014, EngineSize = 6.0},
                new PrivatePassengerCar { Maker = "Hyundai", Model = "i30", ModelYear = 2017, EngineSize = 1.6},
                new PrivatePassengerCar { Maker = "Volkswagen", Model = "Golf 2 GTI 16v", ModelYear = 2012, EngineSize = 2.0},
                new PrivatePassengerCar { Maker = "Peugeot", Model = "206+", ModelYear = 2013, EngineSize = 1.4},
                new PrivatePassengerCar { Maker = "Alfa Romeo", Model = "156", ModelYear = 2014, EngineSize = 2.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "530d", ModelYear = 2017, EngineSize = 3.0},
                new PrivatePassengerCar { Maker = "Audi", Model = "S8", ModelYear = 2012, EngineSize = 4.8},
                new PrivatePassengerCar { Maker = "Mercedes", Model = "G500", ModelYear = 2013, EngineSize = 5.0},
                new PrivatePassengerCar { Maker = "BMW", Model = "M5 Competizion", ModelYear = 2014, EngineSize = 6.0},
                new PrivatePassengerCar { Maker = "Hyundai", Model = "i30", ModelYear = 2017, EngineSize = 1.6},
                new PrivatePassengerCar { Maker = "Volkswagen", Model = "Golf 2 GTI 16v", ModelYear = 2012, EngineSize = 2.0},
                new PrivatePassengerCar { Maker = "Peugeot", Model = "206+", ModelYear = 2013, EngineSize = 1.4},
                new PrivatePassengerCar { Maker = "Alfa Romeo", Model = "156", ModelYear = 2014, EngineSize = 2.0}
            };
            Auctions = new ObservableCollection<Vehicle>(auctions);
        }
    }
}
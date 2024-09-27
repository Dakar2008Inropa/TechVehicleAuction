using AuctionData.Models.VehicleModels;
using Avalonia.Media.Imaging;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TechAuction.Utilities;

namespace TechAuction.ViewModels
{
    public class ShowAuctionViewModel : ViewModelBase
    {

        private string? _CarTitle;

        private string? _Maker;
        private string? _Model;
        private string? _Mileage;
        private string? _LicensePlate;
        private string? _ModelYear;
        private string? _TowingHitch;
        private string? _LicenseType;
        private string? _EngineSize;
        private string? _FuelType;
        private string? _FuelEconomy;
        private string? _FuelCapacity;
        private string? _EnergyClass;

        private string? _SeatCapacity;
        private string? _RequireCommercialLicense;
        private string? _TrunkDimensions;

        private string? _IsoFixMount;

        private string? _Rollcage;
        private string? _FireExtinguisher;
        private string? _RacingSeat;
        private string? _RacingHarness;
        private string? _LoadCapacity;

        private string? _HeavyHeight;
        private string? _HeavyWeigth;
        private string? _HeavyLength;

        private string? _BusSeatingCapacity;
        private string? _BusSleepingCapacity;
        private string? _Toilet;

        private string? _TruckLoadCapacity;

        private string? _ClosingDate;
        private string? _CurrentBid;
        private ObservableCollection<Bitmap>? _Images;

        private bool _ShowPrivatePassengerCar;
        private bool _ShowProfessionalPassengerCar;
        private bool _ShowBus;
        private bool _ShowTruck;

        public HomeViewModel? Parent { get; set; }

        public AuctionData.Models.AuctionModels.Auction? Auction { get; }

        public string? CarTitle
        {
            get => _CarTitle;
            set => this.RaiseAndSetIfChanged(ref _CarTitle, value);
        }

        public string? ClosingDate
        {
            get => _ClosingDate;
            set => this.RaiseAndSetIfChanged(ref _ClosingDate, value);
        }

        public string? CurrentBid
        {
            get => _CurrentBid;
            set => this.RaiseAndSetIfChanged(ref _CurrentBid, value);
        }

        public string? Maker
        {
            get => _Maker;
            set => this.RaiseAndSetIfChanged(ref _Maker, value);
        }

        public string? Model
        {
            get => _Model;
            set => this.RaiseAndSetIfChanged(ref _Model, value);
        }

        public string? Mileage
        {
            get => _Mileage;
            set => this.RaiseAndSetIfChanged(ref _Mileage, value);
        }

        public string? LicensePlate
        {
            get => _LicensePlate;
            set => this.RaiseAndSetIfChanged(ref _LicensePlate, value);
        }

        public string? ModelYear
        {
            get => _ModelYear;
            set => this.RaiseAndSetIfChanged(ref _ModelYear, value);
        }

        public string? TowingHitch
        {
            get => _TowingHitch;
            set => this.RaiseAndSetIfChanged(ref _TowingHitch, value);
        }

        public string? LicenseType
        {
            get => _LicenseType;
            set => this.RaiseAndSetIfChanged(ref _LicenseType, value);
        }

        public string? EngineSize
        {
            get => _EngineSize;
            set => this.RaiseAndSetIfChanged(ref _EngineSize, value);
        }

        public string? FuelType
        {
            get => _FuelType;
            set => this.RaiseAndSetIfChanged(ref _FuelType, value);
        }

        public string? FuelEconomy
        {
            get => _FuelEconomy;
            set => this.RaiseAndSetIfChanged(ref _FuelEconomy, value);
        }

        public string? FuelCapacity
        {
            get => _FuelCapacity;
            set => this.RaiseAndSetIfChanged(ref _FuelCapacity, value);
        }

        public string? EnergyClass
        {
            get => _EnergyClass;
            set => this.RaiseAndSetIfChanged(ref _EnergyClass, value);
        }

        public string? SeatCapacity
        {
            get => _SeatCapacity;
            set => this.RaiseAndSetIfChanged(ref _SeatCapacity, value);
        }

        public string? RequireCommercialLicense
        {
            get => _RequireCommercialLicense;
            set => this.RaiseAndSetIfChanged(ref _RequireCommercialLicense, value);
        }

        public string? TrunkDimensions
        {
            get => _TrunkDimensions;
            set => this.RaiseAndSetIfChanged(ref _TrunkDimensions, value);
        }

        public string? IsoFixMount
        {
            get => _IsoFixMount;
            set => this.RaiseAndSetIfChanged(ref _IsoFixMount, value);
        }

        public string? Rollcage
        {
            get => _Rollcage;
            set => this.RaiseAndSetIfChanged(ref _Rollcage, value);
        }

        public string? FireExtinguisher
        {
            get => _FireExtinguisher;
            set => this.RaiseAndSetIfChanged(ref _FireExtinguisher, value);
        }

        public string? RacingSeat
        {
            get => _RacingSeat;
            set => this.RaiseAndSetIfChanged(ref _RacingSeat, value);
        }

        public string? RacingHarness
        {
            get => _RacingHarness;
            set => this.RaiseAndSetIfChanged(ref _RacingHarness, value);
        }

        public string? LoadCapacity
        {
            get => _LoadCapacity;
            set => this.RaiseAndSetIfChanged(ref _LoadCapacity, value);
        }

        public string? HeavyHeight
        {
            get => _HeavyHeight;
            set => this.RaiseAndSetIfChanged(ref _HeavyHeight, value);
        }

        public string? HeavyWeigth
        {
            get => _HeavyWeigth;
            set => this.RaiseAndSetIfChanged(ref _HeavyWeigth, value);
        }

        public string? HeavyLength
        {
            get => _HeavyLength;
            set => this.RaiseAndSetIfChanged(ref _HeavyLength, value);
        }

        public string? BusSeatingCapacity
        {
            get => _BusSeatingCapacity;
            set => this.RaiseAndSetIfChanged(ref _BusSeatingCapacity, value);
        }

        public string? BusSleepingCapacity
        {
            get => _BusSleepingCapacity;
            set => this.RaiseAndSetIfChanged(ref _BusSleepingCapacity, value);
        }

        public string? Toilet
        {
            get => _Toilet;
            set => this.RaiseAndSetIfChanged(ref _Toilet, value);
        }

        public string? TruckLoadCapacity
        {
            get => _TruckLoadCapacity;
            set => this.RaiseAndSetIfChanged(ref _TruckLoadCapacity, value);
        }

        public bool ShowPrivatePassengerCar
        {
            get => _ShowPrivatePassengerCar;
            set => this.RaiseAndSetIfChanged(ref _ShowPrivatePassengerCar, value);
        }

        public bool ShowProfessionalPassengerCar
        {
            get => _ShowProfessionalPassengerCar;
            set => this.RaiseAndSetIfChanged(ref _ShowProfessionalPassengerCar, value);
        }

        public bool ShowBus
        {
            get => _ShowBus;
            set => this.RaiseAndSetIfChanged(ref _ShowBus, value);
        }

        public bool ShowTruck
        {
            get => _ShowTruck;
            set => this.RaiseAndSetIfChanged(ref _ShowTruck, value);
        }

        public ObservableCollection<Bitmap>? Images
        {
            get => _Images;
            set => this.RaiseAndSetIfChanged(ref _Images, value);
        }



        public ShowAuctionViewModel()
        {
        }

        public ShowAuctionViewModel(AuctionData.Models.AuctionModels.Auction? auctionData, HomeViewModel parent)
        {
            Auction = auctionData;
            Parent = parent;
            CarTitle = $"{Auction!.Vehicle.Maker} {Auction!.Vehicle.Model} ({Auction!.Vehicle.ModelYear})";
            ClosingDate = Auction!.EndDate.ToString("dd-MM-yyyy");
            if (Auction.Bids != null && Auction.Bids.Any())
                CurrentBid = $"DKK {Auction.Bids.Max(x => x.BidAmount).ToString("N2")},-";
            else
                CurrentBid = $"There is no bids...";
            Images = new ObservableCollection<Bitmap>(GetImages(Auction!.Vehicle!.VehicleImages!));

            Maker = Auction.Vehicle.Maker;
            Model = Auction.Vehicle.Model;
            Mileage = $"{Auction.Vehicle.Mileage.ToString("N0")} Km";
            LicensePlate = Auction.Vehicle.LicensePlate;
            ModelYear = Auction.Vehicle.ModelYear.ToString();
            TowingHitch = Auction.Vehicle.Towinghitch ? "Yes" : "No";
            LicenseType = Auction.Vehicle.LicenseType.ToString();
            EngineSize = $"{Auction.Vehicle.EngineSize.ToString("N1")} L";
            FuelType = Auction.Vehicle.FuelType.ToString();
            FuelEconomy = $"{Auction.Vehicle.FuelEconomy.ToString("N1")} Km/L";
            FuelCapacity = $"{Auction.Vehicle.FuelCapacity.ToString("N1")} L";
            EnergyClass = Auction.Vehicle.EnergyClass.ToString();

            ShowPrivatePassengerCar = false;
            ShowProfessionalPassengerCar = false;
            ShowTruck = false;
            ShowBus = false;

            if (Auction.Vehicle is PrivatePassengerCar ppc)
            {
                ShowPrivatePassengerCar = true;
                if (ppc.SeatCapacity == 1)
                {
                    SeatCapacity = $"{ppc.SeatCapacity} Seat";
                }
                else
                {
                    SeatCapacity = $"{ppc.SeatCapacity} Seats";
                }
                RequireCommercialLicense = ppc.RequireCommercialLicense ? "Yes" : "No";
                TrunkDimensions = $"{ppc.TrunkDimensions} m³";
                IsoFixMount = ppc.IsofixMounts ? "Yes" : "No";
            }
            else if (Auction.Vehicle is ProfessionalPassengerCar prpc)
            {
                ShowProfessionalPassengerCar = true;
                if (prpc.SeatCapacity == 1)
                {
                    SeatCapacity = $"{prpc.SeatCapacity} Seat";
                }
                else
                {
                    SeatCapacity = $"{prpc.SeatCapacity} Seats";
                }
                RequireCommercialLicense = prpc.RequireCommercialLicense ? "Yes" : "No";
                TrunkDimensions = $"{prpc.TrunkDimensions} m³";

                Rollcage = prpc.RollCage ? "Yes" : "No";
                FireExtinguisher = prpc.FireExtinguisher ? "Yes" : "No";
                RacingSeat = prpc.RacingSeat ? "Yes" : "No";
                RacingHarness = prpc.RacingHarness ? "Yes" : "No";
                LoadCapacity = $"{prpc.LoadCapacity} Kg";
            }
            else if (Auction.Vehicle is Bus bus)
            {
                ShowBus = true;

                HeavyHeight = $"{bus.Height} m";
                HeavyWeigth = $"{bus.Weight} Kg";
                HeavyLength = $"{bus.Length} m";

                if (bus.SeatingCapacity == 1)
                {
                    BusSeatingCapacity = $"{bus.SeatingCapacity} Seat";
                }
                else
                {
                    BusSeatingCapacity = $"{bus.SeatingCapacity} Seats";
                }
                if (bus.SleepingCapacity == 1)
                {
                    BusSleepingCapacity = $"{bus.SleepingCapacity} Bed";
                }
                else
                {
                    BusSleepingCapacity = $"{bus.SleepingCapacity} Beds";
                }
                Toilet = bus.Toilet ? "Yes" : "No";
            }
            else if (Auction.Vehicle is Truck truck)
            {
                ShowTruck = true;

                HeavyHeight = $"{truck.Height} m";
                HeavyWeigth = $"{truck.Weight} Kg";
                HeavyLength = $"{truck.Length} m";

                TruckLoadCapacity = $"{truck.LoadCapacity} Kg";
            }
        }

        private static List<Bitmap> GetImages(List<VehicleImage> vehicleImages)
        {
            List<Bitmap> bitmaps = new List<Bitmap>();
            foreach (var vehicle in vehicleImages)
            {
                bitmaps.Add(Helper.Vehicle.Base64ToBitmap(vehicle.Image!));
            }
            return bitmaps;
        }
    }
}
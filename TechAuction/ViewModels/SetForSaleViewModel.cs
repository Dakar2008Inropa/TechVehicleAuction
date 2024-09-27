using AuctionData.Models.Database;
using AuctionData.Models.UserModels;
using AuctionData.Models.VehicleModels;
using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using TechAuction.Views;

namespace TechAuction.ViewModels
{
    public class SetForSaleViewModel : ViewModelBase
    {
        private string? _maker;
        private string? _model;
        private int? _mileage;
        private string? _licensePlate;
        private int? _modelYear;
        private bool _towingHitch;

        private decimal _minimumAmount;
        private DateTimeOffset _endDateOffset;

        private FuelType _fuelType;
        private int? _fuelEconomy;
        private int? _fuelCapacity;

        private double? _trunkWidth;
        private double? _trunkHeight;
        private double? _trunkLength;
        private int? _passengerCarSeatCapacity;
        private bool _requireCommercialLicense;
        private bool _isofixMounts;

        private bool _rollCage;
        private bool _fireExtinguisher;
        private bool _racingSeat;
        private bool _racingHarness;
        private int? _professionalPassengerCarLoadCapacity;

        private int? _HeavyHeight;
        private int? _HeavyWeight;
        private int? _HeavyLength;

        private int? _truckLoadCapacity;

        private int? _busSeatCapacity;
        private int? _busSleepCapacity;
        private bool _busToilet;


        private int _selectedVehicleTypeIndex;
        private bool _isHeavyVehicleGroupVisible;
        private bool _isBusGroupVisible;
        private bool _isTruckGroupVisible;
        private bool _isPassengerCarGroupVisible;
        private bool _isPrivatePassengerCarGroupVisible;
        private bool _isProfessionalPassengerCarGroupVisible;
        private double _minEngineSize;
        private double _maxEngineSize;
        private double _engineSize;
        private bool _showErrorText;
        private string? _errorText;
        private List<VehicleImage> _vehicleImages = new List<VehicleImage>();

        public HomeViewModel Parent { get; set; }


        public int SelectedVehicleTypeIndex
        {
            get => _selectedVehicleTypeIndex;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedVehicleTypeIndex, value);

                switch (SelectedVehicleTypeIndex)
                {
                    case 0:
                        IsHeavyVehicleGroupVisible = false;
                        IsBusGroupVisible = false;
                        IsTruckGroupVisible = false;

                        IsPassengerCarGroupVisible = true;
                        IsPrivatePassengerCarGroupVisible = true;
                        IsProfessionalPassengerCarGroupVisible = false;

                        MinEngineSize = 0.7;
                        MaxEngineSize = 10.0;
                        EngineSize = 0.7;

                        break;
                    case 1:
                        IsHeavyVehicleGroupVisible = false;
                        IsBusGroupVisible = false;
                        IsTruckGroupVisible = false;

                        IsPassengerCarGroupVisible = true;
                        IsPrivatePassengerCarGroupVisible = false;
                        IsProfessionalPassengerCarGroupVisible = true;

                        MinEngineSize = 0.7;
                        MaxEngineSize = 10.0;
                        EngineSize = 0.7;

                        break;
                    case 2:
                        IsHeavyVehicleGroupVisible = true;
                        IsBusGroupVisible = false;
                        IsTruckGroupVisible = true;

                        IsPassengerCarGroupVisible = false;
                        IsPrivatePassengerCarGroupVisible = false;
                        IsProfessionalPassengerCarGroupVisible = false;

                        MinEngineSize = 4.2;
                        MaxEngineSize = 15.0;
                        EngineSize = 4.2;

                        break;
                    case 3:
                        IsHeavyVehicleGroupVisible = true;
                        IsBusGroupVisible = true;
                        IsTruckGroupVisible = false;

                        IsPassengerCarGroupVisible = false;
                        IsPrivatePassengerCarGroupVisible = false;
                        IsProfessionalPassengerCarGroupVisible = false;

                        MinEngineSize = 4.2;
                        MaxEngineSize = 15.0;
                        EngineSize = 4.2;

                        break;
                }
            }
        }

        public bool IsHeavyVehicleGroupVisible
        {
            get => _isHeavyVehicleGroupVisible;
            set
            {
                this.RaiseAndSetIfChanged(ref _isHeavyVehicleGroupVisible, value);
            }
        }

        public bool IsBusGroupVisible
        {
            get => _isBusGroupVisible;
            set
            {
                this.RaiseAndSetIfChanged(ref _isBusGroupVisible, value);
            }
        }

        public bool IsTruckGroupVisible
        {
            get => _isTruckGroupVisible;
            set
            {
                this.RaiseAndSetIfChanged(ref _isTruckGroupVisible, value);
            }
        }

        public bool IsPassengerCarGroupVisible
        {
            get => _isPassengerCarGroupVisible;
            set
            {
                this.RaiseAndSetIfChanged(ref _isPassengerCarGroupVisible, value);
            }
        }

        public bool IsPrivatePassengerCarGroupVisible
        {
            get => _isPrivatePassengerCarGroupVisible;
            set
            {
                this.RaiseAndSetIfChanged(ref _isPrivatePassengerCarGroupVisible, value);
            }
        }

        public bool IsProfessionalPassengerCarGroupVisible
        {
            get => _isProfessionalPassengerCarGroupVisible;
            set
            {
                this.RaiseAndSetIfChanged(ref _isProfessionalPassengerCarGroupVisible, value);
            }
        }

        public List<VehicleImage> VehicleImages
        {
            get => _vehicleImages;
            set
            {
                this.RaiseAndSetIfChanged(ref _vehicleImages, value);
            }
        }

        public double MinEngineSize
        {
            get => _minEngineSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _minEngineSize, value);
            }
        }

        public double MaxEngineSize
        {
            get => _maxEngineSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _maxEngineSize, value);
            }
        }

        public double EngineSize
        {
            get => _engineSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _engineSize, value);
            }
        }

        public string? Maker
        {
            get => _maker;
            set
            {
                this.RaiseAndSetIfChanged(ref _maker, value);
            }
        }

        public string? Model
        {
            get => _model;
            set
            {
                this.RaiseAndSetIfChanged(ref _model, value);
            }
        }

        public int? Mileage
        {
            get => _mileage;
            set
            {
                this.RaiseAndSetIfChanged(ref _mileage, value);
            }
        }

        public string? LicensePlate
        {
            get => _licensePlate;
            set
            {
                this.RaiseAndSetIfChanged(ref _licensePlate, value);
            }
        }

        public int? ModelYear
        {
            get => _modelYear;
            set
            {
                this.RaiseAndSetIfChanged(ref _modelYear, value);
            }
        }

        public bool TowingHitch
        {
            get => _towingHitch;
            set
            {
                this.RaiseAndSetIfChanged(ref _towingHitch, value);
            }
        }

        public decimal MinimumAmount
        {
            get => _minimumAmount;
            set
            {
                this.RaiseAndSetIfChanged(ref _minimumAmount, value);
            }
        }

        public DateTimeOffset EndDateOffset
        {
            get => _endDateOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref _endDateOffset, value);
            }
        }

        public FuelType FuelType
        {
            get => _fuelType;
            set
            {
                this.RaiseAndSetIfChanged(ref _fuelType, value);
            }
        }

        public int? FuelEconomy
        {
            get => _fuelEconomy;
            set
            {
                this.RaiseAndSetIfChanged(ref _fuelEconomy, value);
            }
        }

        public int? FuelCapacity
        {
            get => _fuelCapacity;
            set
            {
                this.RaiseAndSetIfChanged(ref _fuelCapacity, value);
            }
        }

        public double? TrunkWidth
        {
            get => _trunkWidth;
            set
            {
                this.RaiseAndSetIfChanged(ref _trunkWidth, value);
            }
        }

        public double? TrunkHeight
        {
            get => _trunkHeight;
            set
            {
                this.RaiseAndSetIfChanged(ref _trunkHeight, value);
            }
        }

        public double? TrunkLength
        {
            get => _trunkLength;
            set
            {
                this.RaiseAndSetIfChanged(ref _trunkLength, value);
            }
        }

        public int? PassengerCarSeatCapacity
        {
            get => _passengerCarSeatCapacity;
            set
            {
                this.RaiseAndSetIfChanged(ref _passengerCarSeatCapacity, value);
            }
        }

        public bool RequireCommercialLicense
        {
            get => _requireCommercialLicense;
            set
            {
                this.RaiseAndSetIfChanged(ref _requireCommercialLicense, value);
            }
        }

        public bool IsofixMounts
        {
            get => _isofixMounts;
            set
            {
                this.RaiseAndSetIfChanged(ref _isofixMounts, value);
            }
        }

        public bool RollCage
        {
            get => _rollCage;
            set
            {
                this.RaiseAndSetIfChanged(ref _rollCage, value);
            }
        }

        public bool FireExtinguisher
        {
            get => _fireExtinguisher;
            set
            {
                this.RaiseAndSetIfChanged(ref _fireExtinguisher, value);
            }
        }

        public bool RacingSeat
        {
            get => _racingSeat;
            set
            {
                this.RaiseAndSetIfChanged(ref _racingSeat, value);
            }
        }

        public bool RacingHarness
        {
            get => _racingHarness;
            set
            {
                this.RaiseAndSetIfChanged(ref _racingHarness, value);
            }
        }

        public int? ProfessionalPassengerCarLoadCapacity
        {
            get => _professionalPassengerCarLoadCapacity;
            set
            {
                this.RaiseAndSetIfChanged(ref _professionalPassengerCarLoadCapacity, value);
            }
        }

        public int? HeavyHeight
        {
            get => _HeavyHeight;
            set
            {
                this.RaiseAndSetIfChanged(ref _HeavyHeight, value);
            }
        }

        public int? HeavyWeight
        {
            get => _HeavyWeight;
            set
            {
                this.RaiseAndSetIfChanged(ref _HeavyWeight, value);
            }
        }

        public int? HeavyLength
        {
            get => _HeavyLength;
            set
            {
                this.RaiseAndSetIfChanged(ref _HeavyLength, value);
            }
        }

        public int? TruckLoadCapacity
        {
            get => _truckLoadCapacity;
            set
            {
                this.RaiseAndSetIfChanged(ref _truckLoadCapacity, value);
            }
        }

        public int? BusSeatCapacity
        {
            get => _busSeatCapacity;
            set
            {
                this.RaiseAndSetIfChanged(ref _busSeatCapacity, value);
            }
        }

        public int? BusSleepCapacity
        {
            get => _busSleepCapacity;
            set
            {
                this.RaiseAndSetIfChanged(ref _busSleepCapacity, value);
            }
        }

        public bool BusToilet
        {
            get => _busToilet;
            set
            {
                this.RaiseAndSetIfChanged(ref _busToilet, value);
            }
        }

        public bool ShowErrorText
        {
            get => _showErrorText;
            set
            {
                this.RaiseAndSetIfChanged(ref _showErrorText, value);
            }
        }

        public string? ErrorText
        {
            get => _errorText;
            set
            {
                this.RaiseAndSetIfChanged(ref _errorText, value);
            }
        }

        public ReactiveCommand<Unit, Unit>? UploadImageCmd { get; }

        public ReactiveCommand<Unit, Unit>? CreateAuctionCmd { get; }

        public void UploadVehicleImage()
        {
            var uploadImageViewModel = new UploadImageViewModel(new UploadImage());
            uploadImageViewModel.VehicleImageAdded += OnVehicleImageAdded;
            var vehicleWindow = new UploadImage
            {
                DataContext = uploadImageViewModel
            };

            vehicleWindow.Show();
        }

        public void CreateAuction()
        {
            if (Maker == null || Model == null || LicensePlate == null)
            {
                ShowErrorText = true;
                ErrorText = "Please fill in all fields";
                return;
            }

            Vehicle vehicle;

            switch (SelectedVehicleTypeIndex)
            {
                case 0:
                    vehicle = new PrivatePassengerCar
                    {
                        TrunkHeight = TrunkHeight.GetValueOrDefault(),
                        TrunkWidth = TrunkWidth.GetValueOrDefault(),
                        TrunkLength = TrunkLength.GetValueOrDefault(),
                        RequireCommercialLicense = RequireCommercialLicense,
                        Discriminator = "PrivatePassengerCar",
                        IsofixMounts = IsofixMounts
                    };
                    break;
                case 1:
                    vehicle = new ProfessionalPassengerCar
                    {
                        TrunkHeight = TrunkHeight.GetValueOrDefault(),
                        TrunkWidth = TrunkWidth.GetValueOrDefault(),
                        TrunkLength = TrunkLength.GetValueOrDefault(),
                        RequireCommercialLicense = RequireCommercialLicense,
                        RollCage = RollCage,
                        FireExtinguisher = FireExtinguisher,
                        RacingSeat = RacingSeat,
                        RacingHarness = RacingHarness,
                        Discriminator = "ProfessionalPassengerCar",
                        LoadCapacity = ProfessionalPassengerCarLoadCapacity.GetValueOrDefault()
                    };
                    break;
                case 2:
                    vehicle = new Bus
                    {
                        Height = HeavyHeight.GetValueOrDefault(),
                        Weight = HeavyWeight.GetValueOrDefault(),
                        Length = HeavyLength.GetValueOrDefault(),
                        SeatingCapacity = BusSeatCapacity.GetValueOrDefault(),
                        SleepingCapacity = BusSleepCapacity.GetValueOrDefault(),
                        Discriminator = "Bus",
                        Toilet = BusToilet
                    };
                    break;
                case 3:
                    vehicle = new Truck
                    {
                        Height = HeavyHeight.GetValueOrDefault(),
                        Weight = HeavyWeight.GetValueOrDefault(),
                        Length = HeavyLength.GetValueOrDefault(),
                        Discriminator = "Truck",
                        LoadCapacity = TruckLoadCapacity.GetValueOrDefault()
                    };
                    break;
                default:
                    throw new InvalidOperationException("Unknown vehicle type");
            }

            vehicle.Maker = Maker;
            vehicle.Model = Model;
            vehicle.Mileage = Mileage.GetValueOrDefault();
            vehicle.LicensePlate = LicensePlate;
            vehicle.ModelYear = ModelYear.GetValueOrDefault();
            vehicle.Towinghitch = TowingHitch;
            vehicle.FuelType = FuelType;
            vehicle.FuelEconomy = FuelEconomy.GetValueOrDefault();
            vehicle.FuelCapacity = FuelCapacity.GetValueOrDefault();
            vehicle.EngineSize = EngineSize;

            vehicle.VehicleImages = VehicleImages;

            foreach (VehicleImage image in vehicle.VehicleImages)
            {
                log.Info($"image.Image: {image.Image}");
            }

            AuctionData.Models.AuctionModels.Auction auction = new AuctionData.Models.AuctionModels.Auction();

            auction.MinimumAmount = MinimumAmount;
            auction.EndDate = EndDateOffset.DateTime;

            Database.Vehicle.CreateVehicleAndAuction(vehicle, auction, GetCurrentUserId());

            Parent.CurrentPage = new AuctionViewModel(Parent);
        }

        public List<FuelType> FuelTypes { get; }

        public SetForSaleViewModel(HomeViewModel parent)
        {
            SelectedVehicleTypeIndex = 0;
            IsHeavyVehicleGroupVisible = false;
            IsBusGroupVisible = false;
            IsTruckGroupVisible = false;
            IsPassengerCarGroupVisible = true;
            IsPrivatePassengerCarGroupVisible = true;
            IsProfessionalPassengerCarGroupVisible = false;
            Mileage = 1;
            HeavyWeight = 1;
            ShowErrorText = false;
            BusSeatCapacity = 1;
            BusSleepCapacity = 1;
            ModelYear = 1885;
            MinimumAmount = 1;
            EndDateOffset = new DateTimeOffset(DateTime.Now);
            FuelEconomy = 1;
            FuelCapacity = 1;
            HeavyHeight = 1;
            HeavyLength = 1;
            PassengerCarSeatCapacity = 1;
            ProfessionalPassengerCarLoadCapacity = 1;
            EngineSize = 0.7;
            FuelTypes = Enum.GetValues(typeof(FuelType)).Cast<FuelType>().ToList();

            UploadImageCmd = ReactiveCommand.Create(UploadVehicleImage);

            CreateAuctionCmd = ReactiveCommand.Create(CreateAuction);
            Parent = parent;
        }

        private void OnVehicleImageAdded(object? sender, VehicleImage image)
        {
            log.Info($"VehicleImage.{nameof(VehicleImage.Image)}: {image.Image}");
            log.Info($"VehicleImage.{nameof(VehicleImage.Description)}: {image.Description}");
            log.Info($"VehicleImage.{nameof(VehicleImage.ImageWidth)}: {image.ImageWidth}");
            log.Info($"VehicleImage.{nameof(VehicleImage.ImageHeight)}: {image.ImageHeight}");

            VehicleImages = new List<VehicleImage>(VehicleImages) { image };
        }

        private static int GetCurrentUserId()
        {
            string? currentU = (string?)Application.Current!.Resources["CurrentUser"];

            User currentUser = Database.User.GetUser(currentU);

            return currentUser.Id;
        }
    }
}
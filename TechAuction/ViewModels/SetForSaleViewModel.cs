using ReactiveUI;
using System.Reactive;
using TechAuction.Views;

namespace TechAuction.ViewModels
{
    public class SetForSaleViewModel : ViewModelBase
    {
        private int _selectedVehicleTypeIndex;
        private bool _isHeavyVehicleGroupVisible;
        private bool _isBusGroupVisible;
        private bool _isTruckGroupVisible;
        private bool _isPassengerCarGroupVisible;
        private bool _isPrivatePassengerCarGroupVisible;
        private bool _isProfessionalPassengerCarGroupVisible;
        private decimal _minEngineSize;
        private decimal _maxEngineSize;
        private decimal _engineSize;


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

                        MinEngineSize = (decimal)0.7;
                        MaxEngineSize = (decimal)10.0;
                        EngineSize = (decimal)0.7;

                        break;
                    case 1:
                        IsHeavyVehicleGroupVisible = false;
                        IsBusGroupVisible = false;
                        IsTruckGroupVisible = false;

                        IsPassengerCarGroupVisible = true;
                        IsPrivatePassengerCarGroupVisible = false;
                        IsProfessionalPassengerCarGroupVisible = true;

                        MinEngineSize = (decimal)0.7;
                        MaxEngineSize = (decimal)10.0;
                        EngineSize = (decimal)0.7;

                        break;
                    case 2:
                        IsHeavyVehicleGroupVisible = true;
                        IsBusGroupVisible = false;
                        IsTruckGroupVisible = true;

                        IsPassengerCarGroupVisible = false;
                        IsPrivatePassengerCarGroupVisible = false;
                        IsProfessionalPassengerCarGroupVisible = false;

                        MinEngineSize = (decimal)4.2;
                        MaxEngineSize = (decimal)15.0;
                        EngineSize = (decimal)4.2;

                        break;
                    case 3:
                        IsHeavyVehicleGroupVisible = true;
                        IsBusGroupVisible = true;
                        IsTruckGroupVisible = false;

                        IsPassengerCarGroupVisible = false;
                        IsPrivatePassengerCarGroupVisible = false;
                        IsProfessionalPassengerCarGroupVisible = false;

                        MinEngineSize = (decimal)4.2;
                        MaxEngineSize = (decimal)15.0;
                        EngineSize = (decimal)4.2;

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

        public decimal MinEngineSize
        {
            get => _minEngineSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _minEngineSize, value);
            }
        }

        public decimal MaxEngineSize
        {
            get => _maxEngineSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _maxEngineSize, value);
            }
        }

        public decimal EngineSize
        {
            get => _engineSize;
            set
            {
                this.RaiseAndSetIfChanged(ref _engineSize, value);
            }
        }

        public ReactiveCommand<Unit, Unit>? UploadImageCmd { get; }

        public static void UploadVehicleImage()
        {
            UploadImage uploadImage = new UploadImage();
            uploadImage.Show();
        }

        public SetForSaleViewModel()
        {
            SelectedVehicleTypeIndex = 0;
            IsHeavyVehicleGroupVisible = false;
            IsBusGroupVisible = false;
            IsTruckGroupVisible = false;
            IsPassengerCarGroupVisible = true;
            IsPrivatePassengerCarGroupVisible = true;
            IsProfessionalPassengerCarGroupVisible = false;
            EngineSize = (decimal)0.7;

            UploadImageCmd = ReactiveCommand.Create(UploadVehicleImage);
        }
    }
}
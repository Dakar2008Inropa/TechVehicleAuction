using AuctionData.Models.VehicleModels;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Threading.Tasks;
using TechAuction.Views;

namespace TechAuction.ViewModels
{
    public class UploadImageViewModel : ViewModelBase
    {
        public event EventHandler<VehicleImage>? VehicleImageAdded;

        private string? _vehicleImage;
        private string? _vehicleDescription;
        private int _vehicleImageWidth;
        private int _vehicleImageHeight;
        private string? _vehicleImageBase64;

        public readonly UploadImage? _view;


        public ReactiveCommand<Unit, Unit> SubmitCommand { get; }
        public ReactiveCommand<Unit, Unit> ChooseImageCommand { get; }
        public Func<IStorageProvider>? RequestStorageProvider { get; set; }
        public Interaction<string, Unit> ShowErrorMessage { get; }

        public string? VehicleImage
        {
            get => _vehicleImage;
            set => this.RaiseAndSetIfChanged(ref _vehicleImage, value);
        }

        public string? VehicleDescription
        {
            get => _vehicleDescription;
            set => this.RaiseAndSetIfChanged(ref _vehicleDescription, value);
        }

        public int VehicleImageWidth
        {
            get => _vehicleImageWidth;
            set => this.RaiseAndSetIfChanged(ref _vehicleImageWidth, value);
        }

        public int VehicleImageHeight
        {
            get => _vehicleImageHeight;
            set => this.RaiseAndSetIfChanged(ref _vehicleImageHeight, value);
        }

        public string? VehicleImageBase64
        {
            get => _vehicleImageBase64;
            set => this.RaiseAndSetIfChanged(ref _vehicleImageBase64, value);
        }


        public UploadImageViewModel(UploadImage view)
        {
            SubmitCommand = ReactiveCommand.Create(OnSubmit);
            ChooseImageCommand = ReactiveCommand.CreateFromTask(ChooseImage);
            ShowErrorMessage = new Interaction<string, Unit>();
            _view = view;
        }

        public UploadImageViewModel()
        {
            SubmitCommand = ReactiveCommand.Create(OnSubmit);
            ChooseImageCommand = ReactiveCommand.CreateFromTask(ChooseImage);
            ShowErrorMessage = new Interaction<string, Unit>();
        }



        private void OnSubmit()
        {
            if (string.IsNullOrEmpty(VehicleImageBase64))
            {
                ShowErrorMessage.Handle("Please select an image for the vehicle.");
            }
            else
            {
                VehicleImage vi = new VehicleImage();
                vi.Image = VehicleImageBase64;
                vi.ImageHeight = VehicleImageHeight;
                vi.ImageWidth = VehicleImageWidth;
                vi.Description = VehicleDescription;

                VehicleImageAdded?.Invoke(this, vi);
            }
        }

        private async Task ChooseImage()
        {
            IStorageProvider? storage = _view!.StorageProvider;

            if (storage == null)
                return;

            var result = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select a vehicle image",
                FileTypeFilter = new List<FilePickerFileType>
        {
            new FilePickerFileType("Image Files")
            {
                Patterns = new[] {"*.png", "*.jpg", "*.jpeg", "*.bmp"}
            }
        },
                AllowMultiple = false
            });

            if (result != null && result.Count > 0)
            {
                var file = result[0];

                var selectedImagePath = file.Path.LocalPath;
                VehicleImage = selectedImagePath;

                using (var stream = await file.OpenReadAsync())
                {
                    var bitmap = new Bitmap(stream);
                    VehicleImageWidth = bitmap.PixelSize.Width;
                    VehicleImageHeight = bitmap.PixelSize.Height;

                    VehicleImageBase64 = ConvertBitmapToBase64(bitmap);
                }
            }
        }

        private static string ConvertBitmapToBase64(Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms);
                byte[] imageBytes = ms.ToArray();

                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}
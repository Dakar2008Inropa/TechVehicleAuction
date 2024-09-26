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
        private string? _ClosingDate;
        private string? _CurrentBid;
        private ObservableCollection<Bitmap>? _Images;

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

        public ObservableCollection<Bitmap>? Images
        {
            get => _Images;
            set => this.RaiseAndSetIfChanged(ref _Images, value);
        }

        public ShowAuctionViewModel()
        {
        }

        public ShowAuctionViewModel(AuctionData.Models.AuctionModels.Auction? auctionData)
        {
            Auction = auctionData;
            CarTitle = $"{Auction!.Vehicle.Maker} {Auction!.Vehicle.Model} ({Auction!.Vehicle.ModelYear})";
            ClosingDate = Auction!.EndDate.ToString("dd-MM-yyyy");
            CurrentBid = $"DKK {Auction.Bids.Max(x => x.BidAmount).ToString("N2")},-";
            Images = new ObservableCollection<Bitmap>(GetImages(Auction!.Vehicle!.VehicleImages!));
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
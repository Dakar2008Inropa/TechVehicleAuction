using AuctionData.Models.AuctionModels;
using AuctionData.Models.Database;
using AuctionData.Models.UserModels;
using Avalonia;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;

namespace TechAuction.ViewModels
{
    public class AuctionViewModel : ViewModelBase
    {
        private bool _VisibleAuctions;
        private bool _VisibleYourAuctions;

        public ObservableCollection<Auction> YourAuctions { get; }
        public ObservableCollection<Auction> Auctions { get; }

        public ReactiveCommand<Auction, Unit> ShowAuctionDetailsCmd { get; }

        public bool VisibleAuctions
        {
            get => _VisibleAuctions;
            set => this.RaiseAndSetIfChanged(ref _VisibleAuctions, value);
        }

        public bool VisibleYourAuctions
        {
            get => _VisibleYourAuctions;
            set => this.RaiseAndSetIfChanged(ref _VisibleYourAuctions, value);
        }

        public AuctionViewModel()
        {
            string? currentU = (string?)Application.Current!.Resources["CurrentUser"];

            User currentUser = Database.User.GetUser(currentU);

            YourAuctions = new ObservableCollection<Auction>(Database.Auction.GetAuctions(currentUser.Id));
            Auctions = new ObservableCollection<Auction>(Database.Auction.GetAuctions());

            VisibleYourAuctions = YourAuctions.Count > 0;
            VisibleAuctions = Auctions.Count > 0;

            ShowAuctionDetailsCmd = ReactiveCommand.Create<Auction>(ShowAuctionDetails);
        }

        private void ShowAuctionDetails(Auction auction)
        {
            AuctionSelected?.Invoke(this, auction);
        }

        public event EventHandler<Auction>? AuctionSelected;
    }
}
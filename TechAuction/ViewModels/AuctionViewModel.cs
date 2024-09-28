using AuctionData.Models.AuctionModels;
using AuctionData.Models.Database;
using AuctionData.Models.UserModels;
using Avalonia;
using Avalonia.Threading;
using DynamicData;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace TechAuction.ViewModels
{
    public class AuctionViewModel : ViewModelBase
    {
        private static readonly SemaphoreSlim _loadSemaphore = new SemaphoreSlim(1, 1);

        private bool _VisibleAuctions;
        private bool _VisibleYourAuctions;

        private bool _LoadingAuctions;

        public HomeViewModel? Parent { get; set; }

        public ObservableCollection<Auction> YourAuctions { get; }
        public ObservableCollection<Auction> Auctions { get; }

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

        public bool LoadingAuctions
        {
            get => _LoadingAuctions;
            set => this.RaiseAndSetIfChanged(ref _LoadingAuctions, value);
        }

        public AuctionViewModel()
        {
            YourAuctions = new ObservableCollection<Auction>();
            Auctions = new ObservableCollection<Auction>();
        }

        public AuctionViewModel(HomeViewModel parent)
        {
            Parent = parent;

            YourAuctions = new ObservableCollection<Auction>();
            Auctions = new ObservableCollection<Auction>();

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await LoadAuctionsAsync();
        }

        private async Task LoadAuctionsAsync()
        {
            if(!await _loadSemaphore.WaitAsync(0))
            {
                log.Info("LoadAuctionAsync is already in progress. Skipping this call.");
                return;
            }

            try
            {
                LoadingAuctions = true;

                string? currentU = (string?)Application.Current!.Resources["CurrentUser"];

                User currentUSer = await Task.Run(() => Database.User.GetUser(currentU));

                List<Auction> userAuctions = await Task.Run(() => Database.Auction.GetAuctions(currentUSer.Id));

                List<Auction> auctions = await Task.Run(() => Database.Auction.GetAuctions());

                await Dispatcher.UIThread.InvokeAsync(() => 
                {
                    YourAuctions.Clear();
                    YourAuctions.AddRange(userAuctions);

                    Auctions.Clear();
                    Auctions.AddRange(auctions);

                    VisibleYourAuctions = YourAuctions.Count > 0;
                    VisibleAuctions = Auctions.Count > 0;
                });
            }
            finally
            {
                _loadSemaphore.Release();
                LoadingAuctions = false;
            }
        }
    }
}
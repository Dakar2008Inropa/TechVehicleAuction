using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using System.Linq;
using TechAuction.Views;

namespace TechAuction.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private ViewModelBase _CurrentPage;
        private readonly AuctionViewModel? _auctionViewModel;
        private readonly SetForSaleViewModel? _setForSaleViewModel;

        public HomeViewModel()
        {
            _auctionViewModel = new AuctionViewModel(this);

            _CurrentPage = _auctionViewModel;

            _setForSaleViewModel = new SetForSaleViewModel(this);
        }

        public HomeViewModel(MainWindowViewModel main)
        {
            _mainWindowViewModel = main;
            _auctionViewModel = new AuctionViewModel(this);
            _setForSaleViewModel = new SetForSaleViewModel(this);

            _CurrentPage = _auctionViewModel;

            _mainWindowViewModel.SetWindowSize(907, 700);
            _mainWindowViewModel.SetWindowMinSize(907, 700);
            _mainWindowViewModel.SetCanResize(true);
            _mainWindowViewModel.CenterizeWindow(true);
        }

        public ViewModelBase CurrentPage
        {
            get => _CurrentPage;
            set => this.RaiseAndSetIfChanged(backingField: ref _CurrentPage, newValue: value, propertyName: nameof(CurrentPage));
        }

        public void ShowAuctionDetails(AuctionData.Models.AuctionModels.Auction auction)
        {
            CurrentPage = new ShowAuctionViewModel(auction, this);
        }

        public void ShowAuctions()
        {
            CurrentPage = new AuctionViewModel(this);
        }

        public void UpdateSelectedNavigationViewItem(int navIndex)
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainWindow = desktop.MainWindow;

                if (mainWindow != null)
                {
                    if(mainWindow.Content is HomeView homeview)
                    {
                        log.Info("Updating selected navigation view item.");
                        homeview.UpdateSelectedNavigationViewItem(navIndex);
                    }
                }
            }
        }
    }
}
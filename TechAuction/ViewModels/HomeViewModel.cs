using ReactiveUI;

namespace TechAuction.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private ViewModelBase _CurrentPage;

        public HomeViewModel()
        {
            _CurrentPage = new AuctionViewModel();
        }

        public HomeViewModel(MainWindowViewModel main)
        {
            _mainWindowViewModel = main;
            _CurrentPage = new AuctionViewModel();

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
    }
}
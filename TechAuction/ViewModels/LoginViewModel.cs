using AuctionData.Models.Database;
using ReactiveUI;
using System.Reactive;

namespace TechAuction.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private string? _Username;
        private string? _Password;
        private bool? _ErrorText;

        public LoginViewModel()
        {
            CreateUserCmd = ReactiveCommand.Create(NavigateToCreateUser);
            HomePageCmd = ReactiveCommand.Create(NavigateToHome);
            ErrorText = false;
        }

        public LoginViewModel(MainWindowViewModel main)
        {
            _mainWindowViewModel = main;
            _mainWindowViewModel.CenterizeWindow(true);
            CreateUserCmd = ReactiveCommand.Create(NavigateToCreateUser);
            HomePageCmd = ReactiveCommand.Create(NavigateToHome);
            ErrorText = false;
        }

        public void NavigateToHome()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorText = true;
                return;
            }
            if (Database.Instance.LoginCheck(Username, Password))
            {
                _mainWindowViewModel?.ChangeView(new HomeViewModel(_mainWindowViewModel));
            }
            else
            {
                ErrorText = true;
            }
        }

        public void NavigateToCreateUser()
        {
            _mainWindowViewModel?.ChangeView(new CreateUserViewModel(_mainWindowViewModel));
        }

        public ReactiveCommand<Unit, Unit>? CreateUserCmd { get; }
        public ReactiveCommand<Unit, Unit>? HomePageCmd { get; }

        public string? Username
        {
            get => _Username;
            set => this.RaiseAndSetIfChanged(backingField: ref _Username, newValue: value, propertyName: nameof(Username));
        }

        public string? Password
        {
            get => _Password;
            set => this.RaiseAndSetIfChanged(backingField: ref _Password, newValue: value, propertyName: nameof(Password));
        }

        public bool? ErrorText
        {
            get => _ErrorText;
            set => this.RaiseAndSetIfChanged(backingField: ref _ErrorText, newValue: value, propertyName: nameof(ErrorText));
        }
    }
}
using ReactiveUI;
using System.Reactive;

namespace TechAuction.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private string? _Username;
        private string? _Password;

        public LoginViewModel()
        {
            CreateUserCmd = ReactiveCommand.Create(NavigateToCreateUser);
            HomePageCmd = ReactiveCommand.Create(NavigateToHome);
        }

        public LoginViewModel(MainWindowViewModel main)
        {
            _mainWindowViewModel = main;
            _mainWindowViewModel.CenterizeWindow(true);
            CreateUserCmd = ReactiveCommand.Create(NavigateToCreateUser);
            HomePageCmd = ReactiveCommand.Create(NavigateToHome);
        }

        public void NavigateToHome()
        {
            _mainWindowViewModel?.ChangeView(new HomeViewModel(_mainWindowViewModel));
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
    }
}
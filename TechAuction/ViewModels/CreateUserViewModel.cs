using ReactiveUI;
using System.Reactive;

namespace TechAuction.ViewModels
{
    public class CreateUserViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;

        public CreateUserViewModel()
        {
        }

        public CreateUserViewModel(MainWindowViewModel main)
        {
            _mainWindowViewModel = main;
            _mainWindowViewModel.CenterizeWindow(true);
            LoginPageCmd = ReactiveCommand.Create(NavigateToLoginPage);
        }

        public void NavigateToLoginPage()
        {
            _mainWindowViewModel?.ChangeView(new LoginViewModel(_mainWindowViewModel));
        }

        public ReactiveCommand<Unit, Unit>? LoginPageCmd { get; }
    }
}
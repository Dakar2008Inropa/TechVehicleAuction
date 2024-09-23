using Avalonia.Controls;
using ReactiveUI;

namespace TechAuction.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _CurrentPage;
        private readonly Window? _window;

        public MainWindowViewModel(Window window)
        {
            _window = window;
            _CurrentPage = new LoginViewModel(this);
        }

        public ViewModelBase CurrentPage
        {
            get => _CurrentPage;
            set => this.RaiseAndSetIfChanged(backingField: ref _CurrentPage, newValue: value, propertyName: nameof(CurrentPage));
        }

        public void ChangeView(ViewModelBase view)
        {
            CurrentPage = view;
        }

        public void SetWindowSize(double width, double height)
        {
            if (_window == null)
                return;

            _window.Width = width;
            _window.Height = height;
        }

        public void SetWindowMinSize(double width, double height)
        {
            if (_window == null)
                return;

            _window.MinHeight = height;
            _window.MinWidth = width;
        }

        public void SetCanResize(bool canResize)
        {
            if (_window == null)
                return;

            _window.CanResize = canResize;
        }

        public void CenterizeWindow(bool centerWindow)
        {
            if (_window == null)
                return;

            _window.WindowStartupLocation = centerWindow ? WindowStartupLocation.CenterScreen : WindowStartupLocation.Manual;
        }

        public string GetWindowSize()
        {
            if (_window == null)
                return string.Empty;

            return $"Width: {_window.Width}, Height: {_window.Height}";
        }
    }
}
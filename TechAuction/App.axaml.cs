using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TechAuction.ViewModels;
using TechAuction.Views;

namespace TechAuction
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var splashScreen = new SplashScreen();

                desktop.MainWindow = splashScreen;
                splashScreen.Show();

                await splashScreen.InitApp();

                var main = new MainWindow();

                main.DataContext = new MainWindowViewModel(main);

                desktop.MainWindow = main;
                main.Show();
                splashScreen.Close();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
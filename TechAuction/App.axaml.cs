using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using log4net;
using Logging;
using System;
using TechAuction.ViewModels;
using TechAuction.Views;

namespace TechAuction
{
    public partial class App : Application
    {
        public static readonly ILog log = Logger.GetLogger(typeof(SetForSaleViewModel));

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            try
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
            catch (Exception ex)
            {
                log.Error("Something went wrong on initializing app", ex);
            }
        }
    }
}
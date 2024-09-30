using AuctionData.Models.Database;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using System;
using System.Threading.Tasks;

namespace TechAuction;

public partial class SplashScreen : Window
{
    const string LoadingTextDB = "Creating Database Tables...";
    const string LoadingCheckDB = "Checking Database Connection...";
    const string LoadingText1 = "Preparing the auction house... Finding the auctioneer!";
    const string LoadingText2 = "Checking tire pressure on all vehicles... Almost ready!";
    const string LoadingText3 = "Polishing the cars, so they shine at the auction...";
    const string LoadingText4 = "Updating the bid signs... Getting ready for excitement!";
    const string LoadingText5 = "Bringing out the gavel... The auction is about to start!";

    const int DefaultTextDelay = 500;

    public SplashScreen()
    {
        InitializeComponent();
    }

    public async Task InitApp()
    {
        var start = DateTime.Now.Ticks;
        var time = start;
        var progressValue = 0;

        while ((time - start) < TimeSpan.TicksPerSecond)
        {
            progressValue++;
            Dispatcher.UIThread.Post(() => LoadingProgressBar.Value = progressValue);
            await Task.Delay(25);
            time = DateTime.Now.Ticks;
        }

        start = time;
        Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingCheckDB);
        await Task.Delay(25);
        if (!Database.Instance.TestConnection())
        {
            var ownerWindow = VisualRoot as AppWindow;
            var td = new TaskDialog
            {
                Title = "",
                Header = "No Database",
                HeaderBackground = new SolidColorBrush(Colors.Red),
                HeaderForeground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(Colors.Red),
                Foreground = new SolidColorBrush(Colors.White),
                SubHeader = "Database Connection Error",
                Content = "The application could not connect to the database. Please check your connection and try again.",
                IconSource = new SymbolIconSource { Symbol = Symbol.Clear },
                FooterVisibility = TaskDialogFooterVisibility.Never
            };

            td.XamlRoot = (Avalonia.Visual?)VisualRoot;

            var result = await td.ShowAsync();

            if (string.IsNullOrEmpty(result.ToString()) || result.ToString() == "None")
            {
                ownerWindow!.Close();
            }
        }
        var limit = TimeSpan.TicksPerSecond * 2;
        while ((time - start) < limit)
        {
            progressValue += 1;
            Dispatcher.UIThread.Post(() => LoadingProgressBar.Value = progressValue);
            await Task.Delay(100);
            time = DateTime.Now.Ticks;
        }
        if (!Database.IsTablesCreated())
        {
            Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingTextDB);
            await Task.Delay(25);
            Database.CreateTables();
        }
        else
        {
            Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingText1);
        }
        await Task.Delay(DefaultTextDelay);
        limit = TimeSpan.TicksPerSecond * 3;
        while ((time - start) < limit)
        {
            progressValue += 1;
            Dispatcher.UIThread.Post(() => LoadingProgressBar.Value = progressValue);
            await Task.Delay(500);
            time = DateTime.Now.Ticks;
        }

        Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingText2);
        await Task.Delay(DefaultTextDelay);
        limit = TimeSpan.TicksPerSecond * 4;
        while ((time - start) < limit)
        {
            progressValue += 1;
            Dispatcher.UIThread.Post(() => LoadingProgressBar.Value = progressValue);
            await Task.Delay(500);
            time = DateTime.Now.Ticks;
        }

        Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingText3);
        await Task.Delay(DefaultTextDelay);
        limit = TimeSpan.TicksPerSecond * 5;
        while ((time - start) < limit)
        {
            progressValue += 1;
            Dispatcher.UIThread.Post(() => LoadingProgressBar.Value = progressValue);
            await Task.Delay(500);
            time = DateTime.Now.Ticks;
        }

        Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingText4);
        await Task.Delay(DefaultTextDelay);
        limit = TimeSpan.TicksPerSecond * 6;
        while ((time - start) < limit)
        {
            progressValue += 1;
            Dispatcher.UIThread.Post(() => LoadingProgressBar.Value = progressValue);
            await Task.Delay(500);
            time = DateTime.Now.Ticks;
        }

        Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingText5);
        await Task.Delay(DefaultTextDelay);
        while (progressValue < 100)
        {
            progressValue += 1;
            Dispatcher.UIThread.Post(() => LoadingProgressBar.Value = progressValue);
            await Task.Delay(10);
        }
    }
}
using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace TechAuction;

public partial class SplashScreen : Window
{
    const string LoadingText1 = "Preparing the auction house... Finding the auctioneer!";
    const string LoadingText2 = "Checking tire pressure on all vehicles... Almost ready!";
    const string LoadingText3 = "Polishing the cars, so they shine at the auction...";
    const string LoadingText4 = "Updating the bid signs... Getting ready for excitement!";
    const string LoadingText5 = "Bringing out the gavel... The auction is about to start!";

    const int DefaultTextDelay = 1500;

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
        Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingText1);
        var limit = TimeSpan.TicksPerSecond * 2;
        while ((time - start) < limit)
        {
            progressValue += 1;
            Dispatcher.UIThread.Post(() => LoadingProgressBar.Value = progressValue);
            await Task.Delay(100);
            time = DateTime.Now.Ticks;
        }

        Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingText2);
        await Task.Delay(DefaultTextDelay);
        limit = TimeSpan.TicksPerSecond * 3;
        while ((time - start) < limit)
        {
            progressValue += 1;
            Dispatcher.UIThread.Post(() => LoadingProgressBar.Value = progressValue);
            await Task.Delay(500);
            time = DateTime.Now.Ticks;
        }

        Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingText3);
        await Task.Delay(DefaultTextDelay);
        limit = TimeSpan.TicksPerSecond * 4;
        while ((time - start) < limit)
        {
            progressValue += 1;
            Dispatcher.UIThread.Post(() => LoadingProgressBar.Value = progressValue);
            await Task.Delay(500);
            time = DateTime.Now.Ticks;
        }

        Dispatcher.UIThread.Post(() => LoadingText.Text = LoadingText4);
        await Task.Delay(DefaultTextDelay);
        limit = TimeSpan.TicksPerSecond * 5;
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
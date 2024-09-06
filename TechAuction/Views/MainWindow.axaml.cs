using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;
using TechAuction.ViewModels;

namespace TechAuction.Views
{
    public partial class MainWindow : AppWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new MainWindowViewModel(this);
        }

        private void Window_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.Width = 600;
            this.Height = 600;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
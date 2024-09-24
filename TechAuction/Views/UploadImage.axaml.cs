using AuctionData.Models.VehicleModels;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using TechAuction.ViewModels;

namespace TechAuction.Views;

public partial class UploadImage : Window
{
    public UploadImage()
    {
        InitializeComponent();
        var viewModel = new UploadImageViewModel(this);

        viewModel.RequestStorageProvider = () => this.StorageProvider;

        this.DataContext = viewModel;
        viewModel.VehicleImageAdded += OnVehicleImageAdded;
        viewModel.ShowErrorMessage.RegisterHandler(interaction =>
        {
            ShowDialog(interaction.Input);
        });
    }

    private void ShowDialog(string message)
    {
        var td = new TaskDialog
        {
            Title = "",
            Header = "No Image",
            SubHeader = "No Image Selected",
            Content = message,
            IconSource = new SymbolIconSource { Symbol = Symbol.Clear },
            FooterVisibility = TaskDialogFooterVisibility.Never
        };

        td.XamlRoot = (Avalonia.Visual?)VisualRoot;

        td.ShowAsync();
    }

    private void OnVehicleImageAdded(object? sender, VehicleImage image)
    {
        this.Close();
    }

    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}
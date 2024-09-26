using Avalonia.Controls;
using TechAuction.ViewModels;

namespace TechAuction.Views;

public partial class AuctionView : UserControl
{
    public AuctionView()
    {
        InitializeComponent();
    }

    private void YourAuctions_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (sender is DataGrid datagrid && datagrid.SelectedItem is AuctionData.Models.AuctionModels.Auction auction)
        {
            var viewModel = (AuctionViewModel?)DataContext;
            viewModel!.ShowAuctionDetailsCmd.Execute(auction);
        }
    }

    private void Auctions_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        if (sender is DataGrid datagrid && datagrid.SelectedItem is AuctionData.Models.AuctionModels.Auction auction)
        {
            var viewModel = (AuctionViewModel?)DataContext;
            viewModel!.ShowAuctionDetailsCmd.Execute(auction);
        }
    }
}
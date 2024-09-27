using Avalonia.Controls;
using log4net;
using Logging;
using System;
using TechAuction.ViewModels;

namespace TechAuction.Views;

public partial class AuctionView : UserControl
{
    private static readonly ILog log = Logger.GetLogger(typeof(AuctionView));

    public AuctionView()
    {
        InitializeComponent();
    }

    private void YourAuctions_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        try
        {
            if (sender is DataGrid datagrid && datagrid.SelectedItem is AuctionData.Models.AuctionModels.Auction auction)
            {
                var viewModel = (AuctionViewModel?)DataContext;
                if (viewModel != null)
                {
                    log.Info($"Auction Id: {auction.Id}");
                    log.Info("viewModel is not null, trying to executing ShowAuctionDetailsCmd");
                    viewModel.Parent!.ShowAuctionDetails(auction);
                }
                else
                    log.Error("Viewmodel is null, sorry");
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in YourAuctions_DoubleTapped", ex);
        }
    }

    private void Auctions_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        try
        {
            if (sender is DataGrid datagrid && datagrid.SelectedItem is AuctionData.Models.AuctionModels.Auction auction)
            {
                var viewModel = (AuctionViewModel?)DataContext;
                if (viewModel != null)
                {
                    log.Info($"Auction Id: {auction.Id}");
                    log.Info("viewModel is not null, trying to executing ShowAuctionDetailsCmd");
                    viewModel.Parent!.ShowAuctionDetails(auction);
                }
                else
                    log.Error("Viewmodel is null, sorry");
            }
        }
        catch (Exception ex)
        {
            log.Error("Error in Auctions_DoubleTapped", ex);
        }
    }
}
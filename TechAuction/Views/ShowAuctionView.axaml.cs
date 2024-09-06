using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TechAuction.Views;

public partial class ShowAuctionView : UserControl
{
    public ShowAuctionView()
    {
        InitializeComponent();
    }

    public void NextImage(object source, RoutedEventArgs args)
    {
        AuctionSlide.Next();
    }

    public void PrevImage(object source, RoutedEventArgs args)
    {
        AuctionSlide.Previous();
    }
}
using Avalonia.Controls;
using TechAuction.ViewModels;

namespace TechAuction.Views;

public partial class UploadImage : Window
{
    public UploadImage()
    {
        InitializeComponent();
        var viewModel = new UploadImageViewModel();
        viewModel.CloseWindow = this.Close;

        viewModel.RequestStorageProvider = (sp) => { sp = this.StorageProvider; };

        this.DataContext = viewModel;
    }
}
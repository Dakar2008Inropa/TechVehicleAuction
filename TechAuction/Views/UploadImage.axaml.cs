using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using System.Threading.Tasks;
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

        viewModel.ShowErrorMessage.RegisterHandler(interaction => {
            ShowDialog(interaction.Input);
        });
    }

    private void ShowDialog(string message)
    {
        var ownerWindow = VisualRoot as AppWindow;
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
}
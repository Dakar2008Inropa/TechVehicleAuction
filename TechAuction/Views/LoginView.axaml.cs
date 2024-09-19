using Avalonia.Controls;

namespace TechAuction.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    private static void Usernamefield_AttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        TextBox box = (TextBox)sender!;
        box.Focus();
    }
}
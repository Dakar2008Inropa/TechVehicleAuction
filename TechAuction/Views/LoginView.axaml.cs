using Avalonia.Controls;

namespace TechAuction.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void Usernamefield_AttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        TextBox box = (TextBox)sender!;
        box.Focus();
    }
}
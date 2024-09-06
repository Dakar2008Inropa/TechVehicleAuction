using Avalonia.Controls;

namespace TechAuction.Views;

public partial class CreateUserView : UserControl
{
    public CreateUserView()
    {
        InitializeComponent();
    }

    private void Usernamefield_AttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        TextBox box = (TextBox)sender!;

        box.Focus();
    }
}
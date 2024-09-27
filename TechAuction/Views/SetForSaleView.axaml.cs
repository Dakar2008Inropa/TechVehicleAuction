using Avalonia.Controls;
using System;

namespace TechAuction.Views;

public partial class SetForSaleView : UserControl
{
    public SetForSaleView()
    {
        InitializeComponent();

        ModelYearField.Value = DateTime.Now.Year;
    }

    private static void Maker_AttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        TextBox box = (TextBox)sender!;
        box.Focus();
    }
}
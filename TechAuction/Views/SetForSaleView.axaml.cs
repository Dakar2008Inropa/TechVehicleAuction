using Avalonia.Controls;
using System;
using System.Globalization;
using System.Threading;

namespace TechAuction.Views;

public partial class SetForSaleView : UserControl
{
    public SetForSaleView()
    {
        InitializeComponent();

        ModelYearField.Value = DateTime.Now.Year;
        SetCulture("en-us");
    }

    private static void SetCulture(string cultureString)
    {
        CultureInfo ci = CultureInfo.CreateSpecificCulture(cultureString);
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
        CultureInfo.DefaultThreadCurrentCulture = ci;
        CultureInfo.DefaultThreadCurrentUICulture = ci;
    }
}
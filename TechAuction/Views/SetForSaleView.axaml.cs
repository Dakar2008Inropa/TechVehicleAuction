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

    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    HeavyVehicleGroup.IsVisible = false;
                    BusGroup.IsVisible = false;
                    TruckGroup.IsVisible = false;

                    PassengerCarGroup.IsVisible = true;
                    PrivatePassengerCarGroup.IsVisible = true;
                    ProfessionalPassengerCarGroup.IsVisible = false;
                    break;
                case 1:
                    HeavyVehicleGroup.IsVisible = false;
                    BusGroup.IsVisible = false;
                    TruckGroup.IsVisible = false;

                    PassengerCarGroup.IsVisible = true;
                    PrivatePassengerCarGroup.IsVisible = false;
                    ProfessionalPassengerCarGroup.IsVisible = true;
                    break;
                case 2:
                    HeavyVehicleGroup.IsVisible = true;
                    BusGroup.IsVisible = false;
                    TruckGroup.IsVisible = true;

                    PassengerCarGroup.IsVisible = false;
                    PrivatePassengerCarGroup.IsVisible = false;
                    ProfessionalPassengerCarGroup.IsVisible = false;
                    break;
                case 3:
                    HeavyVehicleGroup.IsVisible = true;
                    BusGroup.IsVisible = true;
                    TruckGroup.IsVisible = false;

                    PassengerCarGroup.IsVisible = false;
                    PrivatePassengerCarGroup.IsVisible = false;
                    ProfessionalPassengerCarGroup.IsVisible = false;
                    break;
            }
        }
    }
}
using Avalonia.Controls;
using FluentAvalonia.Core;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Windowing;
using System;
using TechAuction.ViewModels;

namespace TechAuction.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();

        DataContext = new HomeViewModel();

        var nv = this.FindControl<NavigationView>("TopMenu");
        if (nv != null)
        {
            nv.SelectionChanged += NavigationView_SelectionChanged;
            nv.SelectedItem = nv.MenuItems.ElementAt(0);
        }
    }

    private async void NavigationView_SelectionChanged(object? sender, NavigationViewSelectionChangedEventArgs e)
    {
        if (e.SelectedItem is NavigationViewItem nvi)
        {
            if (nvi.Content?.ToString() == "Logout")
            {
                var ownerWindow = VisualRoot as AppWindow;

                var td = new TaskDialog
                {
                    Title = "Logout",
                    Header = "Logout",
                    SubHeader = "Are you sure you want to logout?",
                    Content = $"Height: {ownerWindow!.Height} Width: {ownerWindow!.Width} \nYou will be logged out of the application.",
                    IconSource = new SymbolIconSource { Symbol = Symbol.Save },
                    FooterVisibility = TaskDialogFooterVisibility.Never,
                    Buttons =
                {
                    TaskDialogButton.YesButton,
                    TaskDialogButton.NoButton
                }
                };

                td.XamlRoot = (Avalonia.Visual?)VisualRoot;

                var result = await td.ShowAsync();

                if (result.ToString() == "Yes")
                {
                    ownerWindow!.Close();
                }

                if (result.ToString() == "No")
                {
                    TopMenu.SelectedItem = TopMenu.MenuItems.ElementAt(0);
                }

                return;
            }

            if (DataContext is HomeViewModel homeViewModel)
            {
                var page = $"TechAuction.Views.{nvi.Tag}View";
                var type = Type.GetType(page);
                if (type != null)
                {
                    var pg = Activator.CreateInstance(type);

                    if (pg != null && sender is NavigationView navigationView)
                    {
                        if (pg is UserControl userControl)
                        {
                            var viewModelType = Type.GetType($"TechAuction.ViewModels.{nvi.Tag}ViewModel");
                            if (viewModelType != null)
                            {
                                var constructor = viewModelType.GetConstructor(new[] { typeof(HomeViewModel) });
                                object? viewModel = null;

                                if (constructor != null)
                                {
                                    viewModel = constructor.Invoke(new object[] { homeViewModel });
                                }
                                else
                                {
                                    viewModel = Activator.CreateInstance(viewModelType);
                                }

                                userControl.DataContext = viewModel;
                            }
                        }

                        navigationView.Content = pg;
                    }
                }
                else
                {
                    Console.WriteLine($"Type '{page}' was not found.");
                }
            }
        }
    }
}
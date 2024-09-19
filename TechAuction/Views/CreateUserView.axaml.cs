using AuctionData.Models.Database;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using System;

namespace TechAuction.Views;

public partial class CreateUserView : UserControl
{
    public CreateUserView()
    {
        InitializeComponent();
    }

    private static void Usernamefield_AttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        TextBox box = (TextBox)sender!;
        box.Focus();
    }

    private async void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(Usernamefield.Text))
            {
                ErrorText.IsVisible = true;
                ErrorText.Text = "Please enter a username";
                Usernamefield.Focus();
                Usernamefield.SelectAll();
                return;
            }
            if (string.IsNullOrEmpty(Passwordfield.Text))
            {
                ErrorText.IsVisible = true;
                ErrorText.Text = "Please enter a password";
                Passwordfield.Focus();
                Passwordfield.SelectAll();
                return;
            }
            if (string.IsNullOrEmpty(ConfirmPasswordfield.Text))
            {
                ErrorText.IsVisible = true;
                ErrorText.Text = "Please confirm your password";
                ConfirmPasswordfield.Focus();
                ConfirmPasswordfield.SelectAll();
                return;
            }
            if (Passwordfield.Text != ConfirmPasswordfield.Text)
            {
                ErrorText.IsVisible = true;
                ErrorText.Text = "Passwords do not match";
                ConfirmPasswordfield.Focus();
                ConfirmPasswordfield.SelectAll();
                return;
            }
            else
            {
                ErrorText.IsVisible = false;
                ErrorText.Text = "";

                string? userType = "";

                if (CorporateUserRadio.IsChecked.GetValueOrDefault())
                {
                    userType = "CorporatedUser";
                }
                else
                {
                    userType = "PrivateUser";
                }
                if (Database.User.CreateUser(Usernamefield.Text!, Passwordfield.Text!, userType))
                {
                    var td = new TaskDialog
                    {
                        Title = "User Created",
                        Header = "User Created",
                        SubHeader = "User Created",
                        Content = "User has been created and you can now login.",
                        IconSource = new SymbolIconSource { Symbol = Symbol.Contact },
                        FooterVisibility = TaskDialogFooterVisibility.Never,
                        Buttons =
                        {
                            TaskDialogButton.OKButton
                        }
                    };

                    td.XamlRoot = (Avalonia.Visual?)VisualRoot;

                    var result = await td.ShowAsync();

                    if (result.ToString() == "OK")
                    {
                        var loginPage = $"TechAuction.Views.LoginView";
                        var type = Type.GetType(loginPage);
                        if (type != null)
                        {
                            var pg = Activator.CreateInstance(type);

                            if (pg is UserControl userControl)
                            {
                                var viewModelType = Type.GetType($"TechAuction.ViewModels.LoginViewModel");
                                if (viewModelType != null)
                                {
                                    var viewModel = Activator.CreateInstance(viewModelType);
                                    userControl.DataContext = viewModel;
                                }
                                this.Content = pg;
                            }
                        }
                    }
                }
                else
                {
                    ErrorText.IsVisible = true;
                    ErrorText.Text = "Could not create user";
                }
            }
        }
        catch (Exception ex)
        {
            ErrorText.IsVisible = true;
            ErrorText.Text = ex.Message;
        }
    }
}
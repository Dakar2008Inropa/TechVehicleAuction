using AuctionData.Models.Database;
using Avalonia.Controls;
using FluentAvalonia.UI.Controls;
using NAudio.Wave;
using System;
using System.IO;
using System.Threading;
using TechAuction.ViewModels;

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
                    userType = "CorporateUser";
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
                        CreateUserViewModel vm = (CreateUserViewModel)DataContext!;
                        vm.NavigateToLoginPage();
                    }
                }
                else
                {
                    ErrorText.IsVisible = true;
                    ErrorText.Text = "Could not create user";
                    ErrorSound();
                }
            }
        }
        catch (Exception ex)
        {
            ErrorText.IsVisible = true;
            ErrorText.Text = ex.Message;
        }
    }

    private static void ErrorSound()
    {
        var basePath = AppContext.BaseDirectory;
        DirectoryInfo baseDir = new DirectoryInfo(basePath);

        var audioFile = Path.Combine(baseDir.Parent!.Parent!.Parent!.FullName, "Assets", "HvaLaverDu_Psykopat.mp3");

        using (var audioFileReader = new AudioFileReader(audioFile))
        using (var outputDevice = new WaveOutEvent())
        {
            outputDevice.Init(audioFileReader);
            outputDevice.Play();
            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(100);
            }
        }
    }
}
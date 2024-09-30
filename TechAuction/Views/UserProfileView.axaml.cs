using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;
using TechAuction.ViewModels;

namespace TechAuction.Views;

public partial class UserProfileView : ReactiveUserControl<UserProfileViewModel>
{
    public UserProfileView()
    {
        InitializeComponent();

        DataContextChanged += (s, e) =>
        {
            if (DataContext is UserProfileViewModel viewModel)
            {
                viewModel.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(UserProfileViewModel.IsOldPasswordFocused) && viewModel.IsOldPasswordFocused)
                    {
                        OldPasswordField.Focus();
                    }
                    if (args.PropertyName == nameof(UserProfileViewModel.IsPostalCodeFocused) && viewModel.IsPostalCodeFocused)
                    {
                        PostalCodeField.Focus();
                    }
                };
            }
        };

        this.WhenActivated(disposables =>
        {
            if (ViewModel != null)
            {
                ViewModel.RequestStorageProvider.RegisterHandler(context =>
                {
                    var appLifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;

                    if (appLifetime?.MainWindow is Window mainWindow)
                    {
                        context.SetOutput(mainWindow.StorageProvider);
                    }
                    else
                    {
                        context.SetOutput(null);
                    }
                }).DisposeWith(disposables);
            }
        });
    }
}
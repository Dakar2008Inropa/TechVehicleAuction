using AuctionData.Models.Database;
using Avalonia;
using NAudio.Wave;
using ReactiveUI;
using System;
using System.IO;
using System.Reactive;
using System.Threading;

namespace TechAuction.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private string? _Username;
        private string? _Password;
        private bool? _ErrorText;

        public LoginViewModel()
        {
            CreateUserCmd = ReactiveCommand.Create(NavigateToCreateUser);
            HomePageCmd = ReactiveCommand.Create(NavigateToHome);
            ErrorText = false;
        }

        public LoginViewModel(MainWindowViewModel main)
        {
            _mainWindowViewModel = main;
            _mainWindowViewModel.CenterizeWindow(true);
            _mainWindowViewModel.SetWindowMinSize(600, 600);
            _mainWindowViewModel.SetWindowSize(600, 600);
            CreateUserCmd = ReactiveCommand.Create(NavigateToCreateUser);
            HomePageCmd = ReactiveCommand.Create(NavigateToHome);
            ErrorText = false;
        }

        public void NavigateToHome()
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                ErrorText = true;
                ErrorSound();
                return;
            }
            if (Database.Instance.LoginCheck(Username, Password))
            {
                var app = Application.Current;
                app!.Resources["CurrentUser"] = Username;

                _mainWindowViewModel?.ChangeView(new HomeViewModel(_mainWindowViewModel));
            }
            else
            {
                ErrorText = true;
                ErrorSound();
            }
        }

        public void NavigateToCreateUser()
        {
            _mainWindowViewModel?.ChangeView(new CreateUserViewModel(_mainWindowViewModel));
        }

        public ReactiveCommand<Unit, Unit>? CreateUserCmd { get; }
        public ReactiveCommand<Unit, Unit>? HomePageCmd { get; }

        public string? Username
        {
            get => _Username;
            set => this.RaiseAndSetIfChanged(backingField: ref _Username, newValue: value, propertyName: nameof(Username));
        }

        public string? Password
        {
            get => _Password;
            set => this.RaiseAndSetIfChanged(backingField: ref _Password, newValue: value, propertyName: nameof(Password));
        }

        public bool? ErrorText
        {
            get => _ErrorText;
            set => this.RaiseAndSetIfChanged(backingField: ref _ErrorText, newValue: value, propertyName: nameof(ErrorText));
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
}
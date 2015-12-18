using BalloonPop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BalloonPop
{
    public sealed partial class MainPage : Page
    {
        public bool GoingLeft { get; set; }
        public bool StandingStill { get; set; }
        public bool IsMoving { get; set; }
        public bool CanFire { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new MainPageViewModel();
            this.DataContext = this.ViewModel;
            this.CanFire = true;
        }

        public MainPageViewModel ViewModel { get; set; }

        public DispatcherTimer Timer { get; set; }

        private void Grid_ManipulationStarted_1(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (e.PointerDeviceType != PointerDeviceType.Touch)
            {
                e.Handled = true;
            }

            this.Timer = new DispatcherTimer();
            this.Timer.Interval = TimeSpan.FromSeconds(1.0 / 25.0);
            this.Timer.Tick += (s, ev) =>
            {
                if (this.GoingLeft)
                {
                    this.ViewModel.MovePlayerLeft();

                }
                else if (!this.GoingLeft)
                {
                    this.ViewModel.MovePlayerRight();
                }
            };
        }

        private void Grid_ManipulationCompleted_1(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.PointerDeviceType != PointerDeviceType.Touch)
            {
                e.Handled = true;
            }

            this.Timer.Stop();
            this.IsMoving = false;
            this.ViewModel.SetPlayerSpriteStanding();
            this.ViewModel.JoystickTop = 485;
            this.ViewModel.JoystickLeft = 110;
        }

        private void Grid_ManipulationDelta_1(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var x = e.Position.X;
            var y = e.Position.Y;

            this.X.Text = x.ToString();
            this.Y.Text = y.ToString();

            if (x > 100 || x < 0 || y < 0 || y > 100)
            {
                this.Timer.Stop();
                this.IsMoving = false;
                this.ViewModel.SetPlayerSpriteStanding();
                return;
            }

            this.ViewModel.JoystickTop = y + 435;
            this.ViewModel.JoystickLeft = x + 60;

            if (0 <= x && x <= 35)
            {
                this.GoingLeft = true;
                this.StandingStill = false;
            }
            else if (65 <= x && x <= 100)
            {
                this.GoingLeft = false;
                this.StandingStill = false;
            }
            else
            {
                this.StandingStill = true;
            }

            if (this.StandingStill)
            {
                this.Timer.Stop();
                this.IsMoving = false;
                this.ViewModel.SetPlayerSpriteStanding();
                return;
            }

            if (!this.IsMoving)
            {
                this.IsMoving = true;
                this.Timer.Start();
            }
        }

        private void FireWithGun(object sender, RoutedEventArgs e)
        {
            if (!this.CanFire)
            {
                return;
            }

            this.ViewModel.SetHookPosition(this.ViewModel.PlayerVM.Left + 20, 645);
            this.Hook.Visibility = Visibility.Visible;
            this.CanFire = false;

            var hookTimer = new DispatcherTimer();
            hookTimer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);

            hookTimer.Tick += (ob, ev) =>
            {
                this.ViewModel.UpdateHook();

                if (this.ViewModel.HookVM.Top <= 0)
                {
                    hookTimer.Stop();
                    this.Hook.Visibility = Visibility.Collapsed;
                    this.CanFire = true;
                }
            };

            hookTimer.Start();
        }
    }
}

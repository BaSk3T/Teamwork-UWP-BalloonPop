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

            // Creates move animation
            this.Timer.Tick += (s, ev) =>
            {
                if (this.GoingLeft && this.IsMoving)
                {
                    this.ViewModel.MovePlayerLeft();

                }
                else if (!this.GoingLeft && this.IsMoving)
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
            
            // Stops animation of player
            this.Timer.Stop();
            this.IsMoving = false;

            // Resets player sprite
            this.ViewModel.SetPlayerSpriteStanding();

            // Returns touch of joystick to default position
            this.ViewModel.JoystickTop = 330;
            this.ViewModel.JoystickLeft = 70;
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

            //Moves touch of joystick to current location
            this.ViewModel.JoystickTop = y + 290;
            this.ViewModel.JoystickLeft = x + 20;

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

            // Show and set position of projectile
            this.ViewModel.SetHookPosition(this.ViewModel.PlayerVM.Left + 10, 390);
            this.Hook.Visibility = Visibility.Visible;
            this.CanFire = false;

            var hookTimer = new DispatcherTimer();
            hookTimer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);

            hookTimer.Tick += (ob, ev) =>
            {
                // Updates position of projectile
                this.ViewModel.UpdateHook();

                // Destroys balloon if interacted with projectile
                if (this.ViewModel.IsBalloonDestroyed())
                {
                    this.Balloon.Visibility = Visibility.Collapsed;
                    this.StopHook(hookTimer);
                }

                if (this.ViewModel.HookVM.Top <= 0)
                {
                    this.StopHook(hookTimer);
                }
            };

            hookTimer.Start();
        }

        private void StopHook(DispatcherTimer hookTimer)
        {
            // Stops animation of projectile
            hookTimer.Stop();
            this.Hook.Visibility = Visibility.Collapsed;
            this.CanFire = true;
        }
    }
}

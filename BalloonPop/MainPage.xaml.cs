using BalloonPop.Data.DataModels;
using BalloonPop.ViewModels;
using BalloonPop.ViewModels.GameObjects.Balloons;
using BalloonPop.ViewModels.GameObjects.Players;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
using Windows.Storage;
using System.Threading.Tasks;
using BalloonPop.Pages;
using System.Text.RegularExpressions;

namespace BalloonPop
{
    public sealed partial class MainPage : Page
    {
        private Accelerometer accelerometer;

        public MainPage()
        {
            this.InitializeComponent();
            this.ViewModel = new MainPageViewModel();
            this.DataContext = this.ViewModel;
            this.BalloonsInitiated = false;
            
            ////Accelerometer
            //this.accelerometer = Accelerometer.GetDefault();
            //this.accelerometer.ReportInterval = 50;
            //this.accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
        }

        public bool BalloonsInitiated { get; set; }

        public MainPageViewModel ViewModel { get; set; }

        public DispatcherTimer PlayerMovementTimer { get; set; }

        public DispatcherTimer WeaponMovementTimer { get; set; }

        private void AnimatePlayerStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (e.PointerDeviceType != PointerDeviceType.Touch)
            {
                e.Handled = true;
            }

            if (!this.BalloonsInitiated)
            {
                this.BalloonsInitiated = true;
                this.ViewModel.PlayerVM.CanFire = true;
                this.StartBalloons();
            }

            this.InitializePlayerMoveAnimationTimerTick();
        }

        private void AnimatePlayerCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.PointerDeviceType != PointerDeviceType.Touch)
            {
                e.Handled = true;
            }

            // Stops animation of player
            this.PlayerMovementTimer.Stop();
            this.ViewModel.PlayerVM.IsMoving = false;

            // Resets player sprite
            this.ViewModel.SetPlayerSpriteStanding();

            // Returns touch of joystick to default position
            this.ViewModel.JoystickVM.TouchTop = JoystickViewModel.OriginalTouchTopPosition;
            this.ViewModel.JoystickVM.TouchLeft = JoystickViewModel.OriginalTouchLeftPosition;
        }

        private void AnimatePlayerDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var x = e.Position.X;
            var y = e.Position.Y;

            if (x > JoystickViewModel.SizeConst || x < 0 || y < 0 || y > JoystickViewModel.SizeConst)
            {
                this.PlayerMovementTimer.Stop();
                this.ViewModel.PlayerVM.IsMoving = false;
                this.ViewModel.SetPlayerSpriteStanding();
                return;
            }

            //Moves touch of joystick to current location
            this.ViewModel.JoystickVM.TouchTop = y + JoystickViewModel.OriginalJoyistickTopPosition - JoystickViewModel.TouchSizeConst / 2; // top of joystick - half of touch
            this.ViewModel.JoystickVM.TouchLeft = x + JoystickViewModel.OriginalJoystickLeftPosition - JoystickViewModel.TouchSizeConst / 2; // left of joystick - half of touch

            if (0 <= x && x <= JoystickViewModel.SizeConst / 2 - JoystickViewModel.TouchSizeConst / 2)
            {
                this.ViewModel.PlayerVM.GoingLeft = true;
                this.ViewModel.PlayerVM.StandingStill = false;
            }
            else if (JoystickViewModel.SizeConst / 2 + JoystickViewModel.TouchSizeConst / 2 <= x && x <= JoystickViewModel.SizeConst)
            {
                this.ViewModel.PlayerVM.GoingLeft = false;
                this.ViewModel.PlayerVM.StandingStill = false;
            }
            else
            {
                this.ViewModel.PlayerVM.StandingStill = true;
            }

            this.MovePlayerIfPossible();
        }

        private void FireWithGun(object sender, RoutedEventArgs e)
        {
            if (!this.ViewModel.PlayerVM.CanFire)
            {
                return;
            }

            // Show and set position of projectile
            this.ViewModel.SetHookPosition(this.ViewModel.PlayerVM.Left + 10, PlayerViewModel.TopPostion);
            this.ViewModel.HookVM.Visible = true;
            this.ViewModel.PlayerVM.CanFire = false;

            this.InitilizeWeaponMoveAnimationTimerTick();

            this.WeaponMovementTimer.Start();
        }

        private void StopHook()
        {
            // Stops animation of projectile
            this.WeaponMovementTimer.Stop();
            this.ViewModel.SetHookPosition(this.ViewModel.PlayerVM.Left + 10, PlayerViewModel.TopPostion);
            this.ViewModel.HookVM.Visible = false;
            this.ViewModel.PlayerVM.CanFire = true;
        }

        private void StartBalloons()
        {
            var fallTimer = new DispatcherTimer();
            fallTimer.Interval = TimeSpan.FromSeconds(1.0 / 50.0);

            double acellerationForce = 1;

            fallTimer.Tick += (ob, ev) =>
            {
                foreach (var currentBalloon in this.ViewModel.Balloons.Balloons)
                {
                    if (this.ViewModel.NumberOfBalloonsAlive == 0)
                    {
                        this.EndGame();
                    }

                    if (currentBalloon.Popped)
                    {
                        continue;
                    }

                    this.ViewModel.MoveBall(currentBalloon);

                    if (this.ViewModel.IsPlayerDestroyed(currentBalloon))
                    {
                        fallTimer.Stop();
                        this.EndGame();
                    }

                    if (currentBalloon.Left <= 0)
                    {
                        currentBalloon.GoingLeft = false;
                    }
                    else if (currentBalloon.Left >= 400 + this.ViewModel.GetSizeOfBalloon(currentBalloon))
                    {
                        currentBalloon.GoingLeft = true;
                    }

                    if (currentBalloon.Top >= 240)
                    {
                        currentBalloon.Top = 240;
                        currentBalloon.CurrentVelocity = this.GetVelocityOfBalloon(currentBalloon);
                    }

                    currentBalloon.Top += currentBalloon.CurrentVelocity;
                    currentBalloon.CurrentVelocity += acellerationForce;
                }
            };

            fallTimer.Start();
        }

        private void InitializePlayerMoveAnimationTimerTick()
        {
            this.PlayerMovementTimer = new DispatcherTimer();
            this.PlayerMovementTimer.Interval = TimeSpan.FromSeconds(1.0 / 25.0);

            // Creates move animation
            this.PlayerMovementTimer.Tick += (s, ev) =>
            {
                if (this.ViewModel.PlayerVM.GoingLeft && this.ViewModel.PlayerVM.IsMoving && this.ViewModel.PlayerVM.IsAlive)
                {
                    this.ViewModel.MovePlayerLeft();

                }
                else if (!this.ViewModel.PlayerVM.GoingLeft && this.ViewModel.PlayerVM.IsMoving && this.ViewModel.PlayerVM.IsAlive)
                {
                    this.ViewModel.MovePlayerRight();
                }
            };
        }

        private void InitilizeWeaponMoveAnimationTimerTick()
        {
            this.WeaponMovementTimer = new DispatcherTimer();
            this.WeaponMovementTimer.Interval = TimeSpan.FromSeconds(1.0 / 30.0);

            this.WeaponMovementTimer.Tick += (ob, ev) =>
            {
                // Updates position of projectile
                this.ViewModel.UpdateHook();

                foreach (var currentBalloon in this.ViewModel.Balloons.Balloons)
                {
                    if (currentBalloon.Popped)
                    {
                        continue;
                    }

                    // Destroys balloon if interacted with projectile
                    if (this.ViewModel.IsBalloonDestroyed(currentBalloon))
                    {
                        currentBalloon.Popped = true;
                        currentBalloon.Visible = false;
                        this.ViewModel.NumberOfBalloonsAlive--;
                        this.StopHook();
                        this.ViewModel.PlayerVM.Score += 10;
                    }
                }

                if (this.ViewModel.HookVM.Top <= 0)
                {
                    this.StopHook();
                }
            };
        }

        private void MovePlayerIfPossible()
        {
            if (this.ViewModel.PlayerVM.StandingStill)
            {
                this.PlayerMovementTimer.Stop();
                this.ViewModel.PlayerVM.IsMoving = false;
                this.ViewModel.SetPlayerSpriteStanding();
                return;
            }

            if (!this.ViewModel.PlayerVM.IsMoving)
            {
                this.ViewModel.PlayerVM.IsMoving = true;
                this.PlayerMovementTimer.Start();
            }
        }

        async private void ReadingChanged(object Accelerometer, AccelerometerReadingChangedEventArgs e)
        {
            if (!this.BalloonsInitiated)
            {
                this.ViewModel.PlayerVM.CanFire = true;
                this.BalloonsInitiated = true;
                this.StartBalloons();
            }

            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AccelerometerReading reading = e.Reading;

                var x = reading.AccelerationX;
                var y = reading.AccelerationY;
                var z = reading.AccelerationZ;

                var pitchAngle = Math.Atan2(-y, Math.Sqrt(x * x + z * z)) * (180 / Math.PI);

                if (pitchAngle < -20)
                {
                    this.ViewModel.PlayerVM.GoingLeft = true;
                    this.ViewModel.PlayerVM.StandingStill = false;
                }
                else if (pitchAngle > 20)
                {
                    this.ViewModel.PlayerVM.GoingLeft = false;
                    this.ViewModel.PlayerVM.StandingStill = false;
                }
                else
                {
                    this.ViewModel.PlayerVM.StandingStill = true;
                }

                this.MovePlayerIfPossible();
            });
        }

        private void EndGame()
        {
            this.ViewModel.PlayerVM.CanFire = false;
            this.ViewModel.PlayerVM.IsAlive = false;
            this.NameOfPlayer.Visibility = Visibility.Visible;
            this.NameSubmiter.Visibility = Visibility.Visible;
        }

        private void EnterName(object sender, RoutedEventArgs e)
        {
            var score = this.ViewModel.PlayerVM.Score;
            var username = this.NameOfPlayer.Text;

            Regex regex = new Regex("^[a-zA-Z0-9]*$");

            if (!regex.IsMatch(username) || username == String.Empty)
            {
                this.WrongName.Visibility = Visibility.Visible;
                return;
            }

            var playerScore = new PlayerScore
            {
                UserName = username,
                Score = score
            };

            this.Frame.Navigate(typeof(ResultPage), playerScore);
        }

        private double GetVelocityOfBalloon(Balloon balloon)
        {
            double velocity = 0;

            if (typeof(BiggestBlueBalloonViewModel) == balloon.GetType())
            {
                velocity = BiggestBlueBalloonViewModel.Velocity;
            }
            else if (typeof(BigBlueBalloonViewModel) == balloon.GetType())
            {
                velocity = BigBlueBalloonViewModel.Velocity;
            }
            
            return velocity;
        }
    }
}

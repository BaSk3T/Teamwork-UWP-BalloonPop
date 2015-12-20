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
            this.InitDb();

            ////Accelerometer
            //this.accelerometer = Accelerometer.GetDefault();
            //this.accelerometer.ReportInterval = 50;
            //this.accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
        }

        public AllBalloons Balloons { get; set; }

        public BiggestBlueBalloonViewModel BlueBalloonVM { get; set; }

        public MainPageViewModel ViewModel { get; set; }

        public DispatcherTimer PlayerMovementTimer { get; set; }

        public DispatcherTimer WeaponMovementTimer { get; set; }

        private void AnimatePlayerStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (e.PointerDeviceType != PointerDeviceType.Touch)
            {
                e.Handled = true;
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
            this.ViewModel.HookVM.Visible = false;
            this.ViewModel.PlayerVM.CanFire = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var fallTimer = new DispatcherTimer();
            fallTimer.Interval = TimeSpan.FromSeconds(1.0 / 50.0);

            double acellerationForce = 1;
            double velocity = 0;
            double topPosition;
            double leftPosition;
            Balloon currentBalloon;

            fallTimer.Tick += (ob, ev) =>
            {
                currentBalloon = this.ViewModel.Balloons.GetFirst();

                topPosition = currentBalloon.Top;
                leftPosition = currentBalloon.Left;

                this.ViewModel.MoveBall(currentBalloon);

                if (this.ViewModel.IsPlayerDestroyed())
                {
                    this.ViewModel.PlayerVM.IsAlive = false;
                    this.EndGame();
                }


                if (leftPosition <= 0)
                {
                    currentBalloon.GoingLeft = false;
                }
                else if (leftPosition >= 400 + BigBlueBalloonViewModel.Size)
                {
                    currentBalloon.GoingLeft = true;
                }

                if (topPosition >= 240)
                {
                    topPosition = 240;
                    velocity = BiggestBlueBalloonViewModel.Velocity;
                }

                topPosition += velocity;
                velocity += acellerationForce;

                currentBalloon.Top = topPosition;
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
                if (this.ViewModel.PlayerVM.GoingLeft && this.ViewModel.PlayerVM.IsMoving)
                {
                    this.ViewModel.MovePlayerLeft();

                }
                else if (!this.ViewModel.PlayerVM.GoingLeft && this.ViewModel.PlayerVM.IsMoving)
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

                // Destroys balloon if interacted with projectile
                if (this.ViewModel.IsBalloonDestroyed())
                {
                    this.ViewModel.Balloons.GetFirst().Visible = true;
                    this.StopHook();
                    this.ViewModel.PlayerVM.Score += 10;
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

        private async void EndGame()
        {
            var score = this.ViewModel.PlayerVM.Score;

            var playerScore = new PlayerScore
            {
                UserName = "Misho",
                Score = score
            };

            await this.InsertPlayerScoreAsync(playerScore);
        }

        private SQLiteAsyncConnection GetDbConnectionAsync()
        {
            var dbFilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite");

            var connectionFactory =
                new Func<SQLiteConnectionWithLock>(
                    () =>
                    new SQLiteConnectionWithLock(
                        new SQLitePlatformWinRT(),
                        new SQLiteConnectionString(dbFilePath, storeDateTimeAsTicks: false)));

            var asyncConnection = new SQLiteAsyncConnection(connectionFactory);

            return asyncConnection;
        }

        private async void InitDb()
        {
            var connection = this.GetDbConnectionAsync();
            await connection.CreateTableAsync<PlayerScore>();
        }

        private async Task<int> InsertPlayerScoreAsync(PlayerScore playerScore)
        {
            var connection = this.GetDbConnectionAsync();
            var result = await connection.InsertAsync(playerScore);
            return result;
        }

        private async Task<PlayerScore> GetPlayerScoreAsync()
        {
            var connection = this.GetDbConnectionAsync();
            var result = await connection.Table<PlayerScore>().FirstOrDefaultAsync();
            return result;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var playerScore = await this.GetPlayerScoreAsync();
            this.Score.Text = playerScore.ToString();
        }
    }
}

using BalloonPop.ViewModels.GameObjects.Balloons;
using BalloonPop.ViewModels.GameObjects.Players;
using BalloonPop.ViewModels.GameObjects.Weapons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonPop.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            this.JoystickVM = new JoystickViewModel();

            this.HookVM = new HookViewModel();

            this.PlayerVM = new PlayerViewModel();

            this.Balloons = new AllBalloons();
            this.Balloons.Add(new BiggestBlueBalloonViewModel { Left = 200, Top = 10 });
            this.Balloons.Add(new BiggestBlueBalloonViewModel { Left = 150, Top = 10, GoingLeft = true });
            this.NumberOfBalloonsAlive = this.Balloons.Balloons.Count;
        }

        public int NumberOfBalloonsAlive { get; set; }

        public JoystickViewModel JoystickVM { get; set; }

        public HookViewModel HookVM { get; set; }

        public PlayerViewModel PlayerVM { get; set; }

        public AllBalloons Balloons { get; set; }

        public void UpdateHook()
        {
            this.HookVM.CurrentSprite = this.HookVM.Sprites[this.HookVM.CurrentFrame % this.HookVM.SpriteFrames];
            this.HookVM.CurrentFrame++;
            this.HookVM.Top -= HookViewModel.Velocity;
        }

        public void SetHookPosition(double left, double top)
        {
            this.HookVM.Left = left;
            this.HookVM.Top = top;
        }

        public void MovePlayerLeft()
        {
            this.PlayerVM.CurrentSprite = this.PlayerVM.WalkingLeftSprites[this.PlayerVM.CurrentFrame % this.PlayerVM.WalkingLeftSprites.Length];
            this.PlayerVM.Left -= PlayerViewModel.Velocity;
            this.PlayerVM.CurrentFrame++;
        }

        public void MovePlayerRight()
        {
            this.PlayerVM.CurrentSprite = this.PlayerVM.WalkingRightSprites[this.PlayerVM.CurrentFrame % this.PlayerVM.WalkingRightSprites.Length];
            this.PlayerVM.Left += PlayerViewModel.Velocity;
            this.PlayerVM.CurrentFrame++;
        }

        public void SetPlayerSpriteStanding()
        {
            this.PlayerVM.CurrentSprite = this.PlayerVM.StandingStillSprite;
        }

        public void MoveBall(Balloon currentBalloon)
        {
            if (currentBalloon.GoingLeft)
            {
                currentBalloon.Left -= Balloon.SideVelocity;
            }
            else
            {
                currentBalloon.Left += Balloon.SideVelocity;
            }
        }

        public bool IsBalloonDestroyed(Balloon balloon)
        {
            var sizeOfBalloon = this.GetSizeOfBalloon(balloon);

            var balloonLeft = this.Balloons.GetFirst().Left;
            var balloonTop = this.Balloons.GetFirst().Top;
            var hookLeft = this.HookVM.Left;
            var hookTop = this.HookVM.Top;

            return (((balloonLeft <= hookLeft && hookLeft <= balloonLeft + sizeOfBalloon)
                || (balloonLeft <=  hookLeft && hookLeft <= balloonLeft + sizeOfBalloon))
                    && (balloonTop <= hookTop && hookTop <= balloonTop + sizeOfBalloon));
        }

        public bool IsPlayerDestroyed(Balloon balloon)
        {
            var sizeOfBalloon = this.GetSizeOfBalloon(balloon);

            var balloonLeft = balloon.Left;
            var balloonTop = balloon.Top;
            var playerLeft = this.PlayerVM.Left;
            var playerTop = this.PlayerVM.Top;

            bool isPlayerHit =
                (balloonLeft <= playerLeft && playerLeft <= balloonLeft + sizeOfBalloon ||
                 balloonLeft <= playerLeft + PlayerViewModel.PlayerWidth && playerLeft + PlayerViewModel.PlayerWidth <= balloonLeft + sizeOfBalloon) &&
                (playerTop <= balloonTop + sizeOfBalloon);

            return isPlayerHit;
        }

        public double GetSizeOfBalloon(Balloon balloon)
        {
            double sizeOfBalloon = 0;

            if (typeof(BiggestBlueBalloonViewModel) == balloon.GetType())
            {
                sizeOfBalloon = BiggestBlueBalloonViewModel.Size;
            }
            else if (typeof(BigBlueBalloonViewModel) == balloon.GetType())
            {
                sizeOfBalloon = BigBlueBalloonViewModel.Size;
            }

            return sizeOfBalloon;
        }
    }
}

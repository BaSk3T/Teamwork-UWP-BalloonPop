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

            this.BlueBalloonVM = new BiggestBlueBalloonViewModel();
            this.BlueBalloonVM.Left = 150;
            this.BlueBalloonVM.Top = 100;

            this.Balloons = new AllBalloons();
            this.Balloons.Add(this.BlueBalloonVM);
            this.Balloons.Add(new BiggestBlueBalloonViewModel { Left = 150, Top = 150 });
        }
        public JoystickViewModel JoystickVM { get; set; }

        public HookViewModel HookVM { get; set; }

        public PlayerViewModel PlayerVM { get; set; }

        public AllBalloons Balloons { get; set; }

        public BiggestBlueBalloonViewModel BlueBalloonVM { get; set; }

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

        public void MoveBall()
        {
            if (this.BlueBalloonVM.GoingLeft)
            {
                this.Balloons.GetFirst().Left -= Balloon.SideVelocity;
            }
            else
            {
                this.Balloons.GetFirst().Left += Balloon.SideVelocity;
            }
        }

        public bool IsBalloonDestroyed()
        {
            var balloonLeft = this.Balloons.GetFirst().Left;
            var balloonTop = this.Balloons.GetFirst().Top;
            var hookLeft = this.HookVM.Left;
            var hookTop = this.HookVM.Top;

            return ((balloonLeft <= hookLeft && hookLeft + HookViewModel.ProjectileWidthConst <= balloonLeft + BiggestBlueBalloonViewModel.Size)
                    && (hookTop <= balloonTop && balloonTop + BiggestBlueBalloonViewModel.Size <= hookTop + HookViewModel.ProjectileHeightConst));
        }
    }
}

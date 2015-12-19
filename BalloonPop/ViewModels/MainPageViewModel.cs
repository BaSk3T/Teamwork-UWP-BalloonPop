using BalloonPop.ViewModels.GameObjects.Balloons;
using BalloonPop.ViewModels.GameObjects.Players;
using BalloonPop.ViewModels.GameObjects.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonPop.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private double joystickLeft;
        private double joystickTop;

        public MainPageViewModel()
        {
            this.JoystickLeft = 70;
            this.JoystickTop = 330;
            this.HookVM = new HookViewModel();
            this.PlayerVM = new PlayerViewModel();
            this.BlueBalloonVM = new BiggestBlueBalloonViewModel();

            this.PlayerVM.Left = 450;
            this.BlueBalloonVM.Left = 150;
            this.BlueBalloonVM.Top = 30;
        }

        public HookViewModel HookVM { get; set; }

        public PlayerViewModel PlayerVM { get; set; }

        public BiggestBlueBalloonViewModel BlueBalloonVM { get; set; }

        public int HookWidth { get { return 20; } }
        public int HookHeight { get { return 420; } }

        public double JoystickLeft
        {
            get
            {
                return this.joystickLeft;
            }
            set
            {
                if (this.joystickLeft == value)
                {
                    return;
                }

                this.joystickLeft = value;
                this.RaisePropertyChanged("JoystickLeft");
            }
        }

        public double JoystickTop
        {
            get
            {
                return this.joystickTop;
            }
            set
            {
                if (this.joystickTop == value)
                {
                    return;
                }

                this.joystickTop = value;
                this.RaisePropertyChanged("JoystickTop");
            }
        }

        public void UpdateHook()
        {
            this.HookVM.CurrentSprite = this.HookVM.Sprites[this.HookVM.CurrentFrame % this.HookVM.SpriteFrames];
            this.HookVM.CurrentFrame++;
            this.HookVM.Top -= this.HookVM.Velocity;
        }

        public void SetHookPosition(double left, double top)
        {
            this.HookVM.Left = left;
            this.HookVM.Top = top;
        }

        public void MovePlayerLeft()
        {
            this.PlayerVM.CurrentSprite = this.PlayerVM.WalkingLeftSprites[this.PlayerVM.CurrentFrame % this.PlayerVM.WalkingLeftSprites.Length];
            this.PlayerVM.Left -= this.PlayerVM.Velocity;
            this.PlayerVM.CurrentFrame++;
        }

        public void MovePlayerRight()
        {
            this.PlayerVM.CurrentSprite = this.PlayerVM.WalkingRightSprites[this.PlayerVM.CurrentFrame % this.PlayerVM.WalkingRightSprites.Length];
            this.PlayerVM.Left += this.PlayerVM.Velocity;
            this.PlayerVM.CurrentFrame++;
        }

        public void SetPlayerSpriteStanding()
        {
            this.PlayerVM.CurrentSprite = this.PlayerVM.StandingStillSprite;
        }

        public bool IsBalloonDestroyed()
        {
            return (this.BlueBalloonVM.Left <= this.HookVM.Left
                     && this.HookVM.Left <= this.BlueBalloonVM.Left + this.BlueBalloonVM.Width)
                     && (this.BlueBalloonVM.Top <= this.HookVM.Top
                     && this.HookVM.Top <= this.BlueBalloonVM.Top + this.BlueBalloonVM.Height);
        }
    }
}

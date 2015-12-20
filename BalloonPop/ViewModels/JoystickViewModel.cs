using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonPop.ViewModels
{
    public class JoystickViewModel : BasicElementMovement
    {
        public const double OriginalJoystickLeftPosition = 30;
        public const double OriginalJoyistickTopPosition = 205;
        public const double OriginalTouchTopPosition = OriginalJoyistickTopPosition + SizeConst / 2 - TouchSizeConst / 2;
        public const double OriginalTouchLeftPosition = OriginalJoystickLeftPosition + SizeConst / 2 - TouchSizeConst / 2;

        public const double SizeConst = 60;
        public const double TouchSizeConst = 20;

        private double touchLeft;
        private double touchTop;

        public JoystickViewModel()
        {
            this.Left = OriginalJoystickLeftPosition;
            this.Top = OriginalJoyistickTopPosition;
            this.TouchLeft = OriginalTouchLeftPosition;
            this.TouchTop = OriginalTouchTopPosition;
        }

        public double Size { get { return SizeConst; } }

        public double TouchSize { get { return TouchSizeConst; } }

        public double TouchLeft
        {
            get
            {
                return this.touchLeft;
            }
            set
            {
                if (this.touchLeft == value)
                {
                    return;
                }

                this.touchLeft = value;
                this.RaisePropertyChanged("TouchLeft");
            }
        }

        public double TouchTop
        {
            get
            {
                return this.touchTop;
            }
            set
            {
                if (this.touchTop == value)
                {
                    return;
                }

                this.touchTop = value;
                this.RaisePropertyChanged("TouchTop");
            }
        }
    }
}

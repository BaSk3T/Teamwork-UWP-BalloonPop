using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonPop.ViewModels.GameObjects.Balloons
{
    public abstract class Balloon : GameMovableObject
    {
        private double width;
        private double height;

        protected Balloon(double velocity, double size)
            :base(velocity)
        {
            this.Height = size;
            this.Width = size;
        }
        public double Width
        {
            get
            {
                return this.width;
            }
            set
            {
                if (this.width == value)
                {
                    return;
                }

                this.width = value;
                this.RaisePropertyChanged("Width");
            }
        }

        public double Height
        {
            get
            {
                return this.height;
            }
            set
            {
                if (this.height == value)
                {
                    return;
                }

                this.height = value;
                this.RaisePropertyChanged("Height");
            }
        }
    }
}

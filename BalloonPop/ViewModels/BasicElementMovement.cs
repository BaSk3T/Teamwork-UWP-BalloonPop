using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonPop.ViewModels
{
    public class BasicElementMovement : ViewModelBase
    {
        private double top;
        private double left;

        public double Left
        {
            get
            {
                return this.left;
            }
            set
            {
                if (this.left == value)
                {
                    return;
                }

                this.left = value;
                this.RaisePropertyChanged("Left");
            }
        }

        public double Top
        {
            get
            {
                return this.top;
            }
            set
            {
                if (this.top == value)
                {
                    return;
                }

                this.top = value;
                this.RaisePropertyChanged("Top");
            }
        }

    }
}

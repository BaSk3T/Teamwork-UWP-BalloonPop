using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonPop.ViewModels.GameObjects.Balloons
{
    public abstract class Balloon : GameMovableObject
    {
        public const double SideVelocity = 7.0;

        protected Balloon()
        {
            this.Visible = true;
            this.Popped = false;
            this.CurrentVelocity = 0;
        }

        public bool Popped { get; set; }

        public double CurrentVelocity { get; set; }
    }
}

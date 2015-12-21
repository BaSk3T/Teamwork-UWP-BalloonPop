using BalloonPop.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalloonPop.ViewModels.GameObjects.Balloons
{
    public class AllBalloons
    {
        private ObservableCollection<Balloon> balloons;

        public AllBalloons()
        {
            this.balloons = new ObservableCollection<Balloon>();
        }

        public ICollection<Balloon> Balloons
        {
            get
            {
                if (this.balloons == null)
                {
                    this.balloons = new ObservableCollection<Balloon>();
                }

                return this.balloons;
            }
            set
            {
                if (this.balloons == null)
                {
                    this.balloons = new ObservableCollection<Balloon>();
                }

                this.balloons.Clear();
                value.ForEach(this.balloons.Add);
            }
        }

        public void Add(Balloon balloon)
        {
            this.balloons.Add(balloon);
        }

        public Balloon GetFirst()
        {
            return this.balloons.FirstOrDefault();
        }
    }
}

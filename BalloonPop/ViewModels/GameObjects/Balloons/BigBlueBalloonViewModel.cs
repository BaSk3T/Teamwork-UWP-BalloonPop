﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace BalloonPop.ViewModels.GameObjects.Balloons
{
    public class BigBlueBalloonViewModel : Balloon
    {
        private const string SpriteName = "BigBlueBalloon";

        public const double Size = 40;
        public const double Velocity = -18.5;

        public BigBlueBalloonViewModel(double left, double top, bool hasChildren, bool goingLeft)
        {
            this.Left = left;
            this.Top = top;
            this.HasChilren = hasChildren;
            this.GoingLeft = goingLeft;

            this.LoadSprites();
            this.CurrentSprite = this.Sprite;
        }

        public BitmapSource Sprite { get; set; }

        public override void LoadSprites()
        {
            this.Sprite = (BitmapSource)Application.Current.Resources[SpriteName];
        }
    }
}

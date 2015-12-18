namespace BalloonPop.ViewModels.GameObjects
{
    using Windows.UI.Xaml.Media.Imaging;

    using BalloonPop.Helpers;

    public abstract class GameMovableObject : ViewModelBase, ISpriteLoader
    {
        private double top;
        private double left;
        private BitmapSource currentSprite;

        protected GameMovableObject(double velocity)
        {
            this.CurrentFrame = 0;
            this.Velocity = velocity;
        }

        public double Velocity { get; set; }

        public int CurrentFrame { get; set; }

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

        public BitmapSource CurrentSprite
        {
            get
            {
                return this.currentSprite;
            }
            set
            {
                if (this.currentSprite == value)
                {
                    return;
                }

                this.currentSprite = value;
                this.RaisePropertyChanged("CurrentSprite");
            }
        }

        public abstract void LoadSprites();
    }
}

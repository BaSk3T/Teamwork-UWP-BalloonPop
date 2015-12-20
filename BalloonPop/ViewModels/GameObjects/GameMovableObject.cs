namespace BalloonPop.ViewModels.GameObjects
{
    using Windows.UI.Xaml.Media.Imaging;

    using BalloonPop.Helpers;

    public abstract class GameMovableObject : BasicElementMovement, ISpriteLoader
    {
        private bool visible;
        private BitmapSource currentSprite;

        protected GameMovableObject()
        {
            this.CurrentFrame = 0;
        }
        
        public bool GoingLeft { get; set; }
        
        public int CurrentFrame { get; set; }

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

        public bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                if (this.visible == value)
                {
                    return;
                }

                this.visible = value;
                this.RaisePropertyChanged("Visible");
            }
        }

        public abstract void LoadSprites();
    }
}

namespace BalloonPop.ViewModels.GameObjects.Players
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Imaging;

    public class PlayerViewModel : GameMovableObject
    {
        private const int WalkingFrames = 4;
        private const string WalkingLeftSpriteName = "WalkingLeft";
        private const string WalkingRightSpriteName = "WalkingRight";
        private const string StandingStillSpriteName = "StandStill";
        public const double TopPostion = 235;
        public const double LeftPosition = 140;

        public const double Velocity = 14;

        public PlayerViewModel()
        {
            this.WalkingLeftSprites = new BitmapSource[WalkingFrames];
            this.WalkingRightSprites = new BitmapSource[WalkingFrames];
            this.LoadSprites();
            this.CurrentSprite = this.StandingStillSprite;
            this.CanFire = true;
            this.Visible = true;
            this.Top = TopPostion;
            this.Left = LeftPosition;
        }
        
        public bool CanFire { get; set; }

        public bool IsMoving { get; set; }

        public bool StandingStill { get; set; }

        public BitmapSource[] WalkingLeftSprites { get; set; }

        public BitmapSource[] WalkingRightSprites { get; set; }

        public BitmapSource StandingStillSprite { get; set; }

        public override void LoadSprites()
        {
            this.StandingStillSprite = (BitmapSource)Application.Current.Resources[StandingStillSpriteName];

            for (int frameCount = 0; frameCount < WalkingFrames; frameCount++)
            {
                this.WalkingLeftSprites[frameCount] = (BitmapSource)Application.Current.Resources[WalkingLeftSpriteName + frameCount];
                this.WalkingRightSprites[frameCount] = (BitmapSource)Application.Current.Resources[WalkingRightSpriteName + frameCount];
            }
        }
    }
}

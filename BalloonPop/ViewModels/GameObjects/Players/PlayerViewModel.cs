namespace BalloonPop.ViewModels.GameObjects.Players
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Imaging;

    public class PlayerViewModel : GameMovableObject
    {
        private const double VelocityConst = 14;
        private const int WalkingFrames = 4;
        private const string WalkingLeftSpriteName = "WalkingLeft";
        private const string WalkingRightSpriteName = "WalkingRight";
        private const string StandingStillSpriteName = "StandStill";

        public PlayerViewModel()
            : base(VelocityConst)
        {
            this.WalkingLeftSprites = new BitmapSource[WalkingFrames];
            this.WalkingRightSprites = new BitmapSource[WalkingFrames];
            this.LoadSprites();
            this.CurrentSprite = this.StandingStillSprite;
        }

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

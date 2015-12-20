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
        public const double PlayerWidth = 45;
        public const double PlayerHeight = 60;

        private int score;

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
            this.SetInitialScore();
            this.IsAlive = true;
        }

        public bool IsAlive { get; set; }

        public bool CanFire { get; set; }

        public bool IsMoving { get; set; }

        public bool StandingStill { get; set; }

        public BitmapSource[] WalkingLeftSprites { get; set; }

        public BitmapSource[] WalkingRightSprites { get; set; }

        public BitmapSource StandingStillSprite { get; set; }

        public int Score
        {
            get
            {
                return this.score;
            }
            set
            {
                if (this.score == value)
                {
                    return;
                }
                this.score = value;
                this.RaisePropertyChanged("Score");
            }
        }

        public override void LoadSprites()
        {
            this.StandingStillSprite = (BitmapSource)Application.Current.Resources[StandingStillSpriteName];

            for (int frameCount = 0; frameCount < WalkingFrames; frameCount++)
            {
                this.WalkingLeftSprites[frameCount] = (BitmapSource)Application.Current.Resources[WalkingLeftSpriteName + frameCount];
                this.WalkingRightSprites[frameCount] = (BitmapSource)Application.Current.Resources[WalkingRightSpriteName + frameCount];
            }
        }

        public void SetInitialScore()
        {
            this.Score = 0;
        }
    }
}

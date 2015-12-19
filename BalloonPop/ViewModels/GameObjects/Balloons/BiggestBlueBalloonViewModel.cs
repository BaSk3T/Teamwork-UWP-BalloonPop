namespace BalloonPop.ViewModels.GameObjects.Balloons
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Imaging;

    public class BiggestBlueBalloonViewModel : Balloon
    {
        private const double Size = 75;
        private const double VelocityConst = 10;
        private const string SpriteName = "BiggestBlueBalloon";

        public BiggestBlueBalloonViewModel()
            :base(VelocityConst, Size)
        {
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

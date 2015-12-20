namespace BalloonPop.ViewModels.GameObjects.Balloons
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Imaging;

    public class BiggestBlueBalloonViewModel : Balloon
    {
        private const string SpriteName = "BiggestBlueBalloon";

        public const double Size = 60;
        public const double Velocity = -20.5;

        public BiggestBlueBalloonViewModel()
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

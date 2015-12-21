namespace BalloonPop.ViewModels.GameObjects.Balloons
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Imaging;

    public class MediumBlueBalloonViewModel : Balloon
    {
        private const string SpriteName = "MediumBlueBalloon";

        public const double Size = 25;
        public const double Velocity = -16.5;

        public MediumBlueBalloonViewModel(double left, double top, bool hasChildren, bool goingLeft)
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

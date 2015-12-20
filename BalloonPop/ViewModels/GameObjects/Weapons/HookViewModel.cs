namespace BalloonPop.ViewModels.GameObjects.Weapons
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Imaging;

    public class HookViewModel : Weapon
    {
        private const string SpriteName = "Hook";
        private const int Frames = 2;

        public const double Velocity = 14;
        public const double ProjectileHeightConst = 420;
        public const double ProjectileWidthConst = 20;

        public HookViewModel()
        {
            this.Sprites = new BitmapSource[Frames];

            this.LoadSprites();
        }

        public double ProjectileHeight { get { return ProjectileHeightConst; } }

        public double ProjectileHWidth { get { return ProjectileWidthConst; } }

        public BitmapSource[] Sprites { get; set; }

        public int SpriteFrames { get { return Frames; } }

        public override void LoadSprites()
        {
            for (int frameCount = 0; frameCount < Frames; frameCount++)
            {
                this.Sprites[frameCount] = (BitmapSource)Application.Current.Resources[SpriteName + frameCount];
            }
        }
    }
}

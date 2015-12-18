namespace BalloonPop.ViewModels.GameObjects.Weapons
{
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Media.Imaging;

    public class HookViewModel : Weapon
    {
        private const double VelocityConst = 14;
        private const string SpriteName = "Hook";
        private const int Frames = 2;

        public HookViewModel()
            : base(VelocityConst)
        {
            this.Sprites = new BitmapSource[Frames];

            this.LoadSprites();
            this.SetProjectileSize();
        }

        public BitmapSource[] Sprites { get; set; }

        public int SpriteFrames { get { return Frames; } }

        public override void LoadSprites()
        {
            for (int frameCount = 0; frameCount < Frames; frameCount++)
            {
                this.Sprites[frameCount] = (BitmapSource)Application.Current.Resources[SpriteName + frameCount];
            }
        }

        private void SetProjectileSize()
        {
            this.ProjectileHeight = this.Sprites[0].PixelHeight;
            this.ProjectileWidth = this.Sprites[0].PixelWidth;
        }
    }
}

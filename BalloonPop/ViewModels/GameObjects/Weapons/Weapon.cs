namespace BalloonPop.ViewModels.GameObjects.Weapons
{
    public abstract class Weapon : GameMovableObject
    {
        protected Weapon(double velocity)
            : base(velocity)
        {
        }

        public int ProjectileWidth { get; set; }

        public int ProjectileHeight { get; set; }
    }
}

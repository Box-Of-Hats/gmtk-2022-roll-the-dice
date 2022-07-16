namespace gmtkjame2022rollthedice
{
    public class TurretModel : Godot.Object, ITurret
    {
        public float RateOfFire { get; set; }
        public int BulletSpeed { get; set; }
        public float RotateSpeed { get; set; }
        public string TopSprite { get; set; }
        public string BottomSprite { get; set; }
        public int Cost { get; set; }
        public int Range { get; set; }
        public int Damage { get; set; }
        public float BulletSize { get; set; }
        public int MaxCollisions { get; set; }
    }
}

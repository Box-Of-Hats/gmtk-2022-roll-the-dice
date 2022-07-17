using gmtkjame2022rollthedice.Interfaces;

namespace gmtkjame2022rollthedice.Models
{
    public class Cannon : ICannon
    {
        public string SpritePath { get; set; }



        public float BulletSize { get; set; } = 1;
        public float RateOfFire { get; set; } = 1;
        public int BulletSpeed { get; set; } = 600;
        public int Damage { get; set; } = 1;
        public float RotateSpeed { get; set; } = 0.05f;
        public int Cost { get; set; } = 100;
        public int Range { get; set; } = 400;
        public int MaxCollisions { get; set; } = 1;
    }
}


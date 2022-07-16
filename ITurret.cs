namespace gmtkjame2022rollthedice
{
    public interface ITurret
    {
        // Function

        /// <summary>
        /// The number of seconds between each shot being fired
        /// </summary>
        float RateOfFire { get; set; }

        int BulletSpeed { get; set; }

        float RotateSpeed { get; set; }
        int Range { get; set; }
        int Damage { get; set; }


        // Sprites
        string TopSprite { get; set; }
        string BottomSprite { get; set; }

        // Misc
        int Cost { get; set; }

    }
}

using gmtkjame2022rollthedice.Interfaces;

namespace gmtkjame2022rollthedice.Models
{
    public class EnemyModel : IHasSpritePath
    {
        public float HealthMultiplier { get; set; } = 1f;

        public string SpritePath { get; set; }

        /// <summary>
        /// e.g 150 (100-500 range)
        /// </summary>
        public int MoveSpeed { get; set; } = 200;

    }
}

using gmtkjame2022rollthedice.Interfaces;
using Godot;

namespace gmtkjame2022rollthedice.Models
{
    public class EnemyModel : IHasTexture
    {
        public float HealthMultiplier { get; set; } = 1f;

        public Texture SpriteTexture { get; set; }

        /// <summary>
        /// e.g 150 (100-500 range)
        /// </summary>
        public int MoveSpeed { get; set; } = 200;

    }
}

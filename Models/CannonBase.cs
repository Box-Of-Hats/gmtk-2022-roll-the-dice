using gmtkjame2022rollthedice.Interfaces;
using Godot;

namespace gmtkjame2022rollthedice.Models
{
    public class CannonBase : IHasCost, IHasRotateSpeed, IHasTexture, IHasRange, IHasDamage
    {
        public float RotateSpeed { get; set; }
        public int Cost { get; set; }
        public int Range { get; set; }
        public int Damage { get; set; }
        public Texture SpriteTexture { get; set; }
    }
}

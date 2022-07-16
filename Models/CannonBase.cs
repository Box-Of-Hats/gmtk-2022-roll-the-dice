using gmtkjame2022rollthedice.Interfaces;

namespace gmtkjame2022rollthedice.Models
{
    public class CannonBase : IHasCost, IHasRotateSpeed, IHasSpritePath, IHasRange, IHasDamage
    {
        public float RotateSpeed { get; set; }
        public int Cost { get; set; }
        public string SpritePath { get; set; }
        public int Range { get; set; }
        public int Damage { get; set; }
    }
}

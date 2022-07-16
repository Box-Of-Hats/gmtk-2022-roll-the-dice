using gmtkjame2022rollthedice.Interfaces;

namespace gmtkjame2022rollthedice.Models
{
    public class CannonBase : IHasCost, IHasRotateSpeed, IHasSpritePath
    {
        public float RotateSpeed { get; set; }
        public int Cost { get; set; }
        public string SpritePath { get; set; }
    }
}

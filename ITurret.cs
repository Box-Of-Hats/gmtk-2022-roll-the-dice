using gmtkjame2022rollthedice.Interfaces;

namespace gmtkjame2022rollthedice
{
    public interface ITurret :
        IHasBulletSpeed,
        IHasRateOfFire,
        IHasDamage,
        IHasCost,
        IHasRotateSpeed,
        IHasRange
    {
        // Sprites
        string TopSprite { get; set; }
        string BottomSprite { get; set; }
    }

}

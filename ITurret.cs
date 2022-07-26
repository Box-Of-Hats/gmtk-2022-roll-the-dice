using gmtkjame2022rollthedice.Interfaces;
using Godot;

namespace gmtkjame2022rollthedice
{
    public interface ITurret :
        IHasBulletSpeed,
        IHasRateOfFire,
        IHasDamage,
        IHasCost,
        IHasRotateSpeed,
        IHasRange,
        IHasBulletSize,
        IHasMaxCollisions
    {
        // Sprites
        Texture TopSprite { get; set; }
        Texture BottomSprite { get; set; }
    }

}

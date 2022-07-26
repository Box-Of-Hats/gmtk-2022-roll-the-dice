using Godot;

namespace gmtkjame2022rollthedice.Interfaces
{
    public interface IHasTexture
    {
        /// <summary>
        /// A pre-loaded sprite texture
        /// </summary>
        Texture SpriteTexture { get; set; }
    }
}

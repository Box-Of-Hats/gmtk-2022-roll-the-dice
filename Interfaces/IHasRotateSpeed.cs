namespace gmtkjame2022rollthedice.Interfaces
{
    public interface IHasRotateSpeed
    {
        /// <summary>
        /// The speed at which the cannon will rotate, in radians per frame. E.g 0.05
        /// </summary>
        float RotateSpeed { get; set; }
    }
}

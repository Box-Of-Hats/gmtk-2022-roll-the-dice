namespace gmtkjame2022rollthedice.Interfaces
{
    public interface IHasRange
    {
        /// <summary>
        /// The range at which a tower can target an enemy
        /// </summary>
        int Range { get; set; }
    }
}

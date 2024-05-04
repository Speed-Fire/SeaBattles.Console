namespace SeaBattles.Console.Models
{
    /// <summary>
    /// Třída reprezentující nastavení hracího pole.
    /// </summary>
    internal class FieldSetup
    {
        /// <summary>
        /// Velikost hracího pole.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Obtížnost hry.
        /// </summary>
        public Difficulty Difficulty { get; set; }
    }
}

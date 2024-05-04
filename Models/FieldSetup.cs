namespace SeaBattles.Console.Models
{
    /// <summary>
    /// Trida reprezentujici nastaveni hraciho pole.
    /// </summary>
    internal class FieldSetup
    {
        /// <summary>
        /// Velikost hraciho pole.
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Obtiznost hry.
        /// </summary>
        public Difficulty Difficulty { get; set; }
    }
}

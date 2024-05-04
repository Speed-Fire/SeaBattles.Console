namespace SeaBattles.Console
{
    /// <summary>
    /// Vycet reprezentujici stav bunky na hracim poli.
    /// </summary>
    public enum CellState
    {
        /// <summary>
        /// Prazdna bunka.
        /// </summary>
        Empty,

        /// <summary>
        /// Bunka obsahuje lod.
        /// </summary>
        Ship,

        /// <summary>
        /// Bunka byla napadena, ale lod nebyla zasazena.
        /// </summary>
        Attacked,

        /// <summary>
        /// Lod v bunce byla zasazena a znicena.
        /// </summary>
        Destroyed,

    }

}

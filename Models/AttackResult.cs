namespace SeaBattles.Console
{
    /// <summary>
    /// Vycet reprezentujici vysledek utoku na lod.
    /// </summary>
    internal enum AttackResult
    {
        /// <summary>
        /// Neuspesny utok.
        /// </summary>
        Failed,

        /// <summary>
        /// Utok minul lod.
        /// </summary>
        Missed,

        /// <summary>
        /// Lod byla zasazena.
        /// </summary>
        Hitten,

        /// <summary>
        /// Lod byla zasazena a znicena.
        /// </summary>
        Destroyed
    }

}

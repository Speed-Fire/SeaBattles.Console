namespace SeaBattles.Console
{
    /// <summary>
    /// Výčet reprezentující výsledek útoku na loď.
    /// </summary>
    internal enum AttackResult
    {
        /// <summary>
        /// Neúspěšný útok.
        /// </summary>
        Failed,

        /// <summary>
        /// Útok minul loď.
        /// </summary>
        Missed,

        /// <summary>
        /// Loď byla zasažena.
        /// </summary>
        Hitten,

        /// <summary>
        /// Loď byla zasažena a zničena.
        /// </summary>
        Destroyed
    }

}

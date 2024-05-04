using SeaBattles.Console.Models;

namespace SeaBattles.Console.FieldFactories
{
    /// <summary>
    /// Rozhrani pro tovarnu na vytvareni bojovych poli.
    /// </summary>
    internal interface IFieldFactory
    {
        /// <summary>
        /// Vytvori bojove pole na zaklade nastaveni.
        /// </summary>
        /// <param name="setup">Nastaveni pole.</param>
        /// <returns>Bojove pole.</returns>
        BattleField CreateBattlefield(FieldSetup setup);
    }
}

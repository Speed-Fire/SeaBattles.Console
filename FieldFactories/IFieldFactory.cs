﻿using SeaBattles.Console.Models;

namespace SeaBattles.Console.FieldFactories
{
    /// <summary>
    /// Rozhrani pro tovarnu na vytvareni hracich poli.
    /// </summary>
    internal interface IFieldFactory
    {
        /// <summary>
        /// Vytvori hraci pole na zaklade nastaveni.
        /// </summary>
        /// <param name="setup">Nastaveni pole.</param>
        /// <returns>Hraci pole.</returns>
        BattleField CreateBattlefield(FieldSetup setup);
    }
}

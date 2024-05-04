using SeaBattles.Console.FieldFillers;
using SeaBattles.Console.Misc;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.FieldFactories
{
    /// <summary>
    /// Tovarna na vytvareni hracih poli pro pocitacoveho protihrace.
    /// </summary>
    internal class ComputerFieldFactory : IFieldFactory
    {
        private const int MAX_BAD_SHIP_PLACING = 3;

        private readonly Random _random = new();

		/// <summary>
		/// Vytvori hraci pole na zaklade zadaneho nastaveni.
		/// </summary>
		/// <param name="setup">Nastaveni pole.</param>
		/// <returns>Hraci pole.</returns>
		public BattleField CreateBattlefield(FieldSetup setup)
        {
            while (true)
            {
                var badIter = 0;

                var filler = FieldFillerFactory.Create(setup.Size);

                while (filler.AvailableShips.Any())
                {
                    // volba souradnic
                    var (x, y) = ChooseCoords(setup.Size);

                    // volba velikosti
                    var size = ChooseSize(filler);

                    // volba smeru
                    var direction = ChooseDirection();

                    // pokus umisteni lode.
                    //  pokud neni mozne rozmistit lod, zvysi pocet chyb
                    if (!filler.PutShip(x, y, size, direction))
                        badIter++;
                    //  pokud lod je uspesne rozmistena, vynyluje pocet chyb
                    else
                        badIter = 0;

                    // pokud pocet chyb prekrocil max povoleny pocet chyb,
                    //  pak ukonci tento cyklus.
                    if (badIter == MAX_BAD_SHIP_PLACING)
                        break;
                }

                // pokud byly rozmistene vsechny lodi, ukonci tvorbu;
                //  jinak smaze pole, a zascne rosmistet lodi znovu.
                if (!filler.AvailableShips.Any())
                    return filler.Build();
            }
        }

        /// <summary>
        /// Vybere smer umisteni lodi.
        /// </summary>
        /// <returns>Smer umisteni lodi.</returns>
        private ShipDirection ChooseDirection()
        {
            return (ShipDirection)_random.Next(0, 2);
        }

        /// <summary>
        /// Vybere velikost lodi na zaklade dostupnych lodi.
        /// </summary>
        /// <param name="filler">Objekt pro plneni pole lodemi.</param>
        /// <returns>Velikost lodi.</returns>
        private ShipSize ChooseSize(FieldFiller filler)
        {
            var availableShips = filler.AvailableShips;

            var keys = availableShips.Keys;

            var pos = _random.Next(0, keys.Count());

            return keys.ElementAt(pos);
        }

        /// <summary>
        /// Vybere nahodne souradnice pro umisteni lodi na poli.
        /// </summary>
        /// <param name="maxSize">Maximalni velikost pole.</param>
        /// <returns>Nahodne souradnice.</returns>
        private static (int, int) ChooseCoords(int maxSize)
        {
            var res = ComputerFieldCoordsGenerator.Generate(maxSize);

            return (res.X, res.Y);
        }
    }
}

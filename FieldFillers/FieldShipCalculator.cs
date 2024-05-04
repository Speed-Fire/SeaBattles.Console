namespace SeaBattles.Console.FieldFillers
{
    /// <summary>
    /// Trida poskytujici staticke metody pro vypocet poctu lodi na poli.
    /// </summary>
    internal static class FieldShipCalculator
    {
        /// <summary>
        /// Vytvori pole poctu lodi a slovnik lodi na zaklade zadaneho pole.
        /// </summary>
        /// <param name="size">Velikost pole.</param>
        /// <param name="field">Pole.</param>
        /// <returns>Koruplat tvoreny pocty lodi a slovnikem lodi.</returns>
        public static (int[,], Dictionary<int, int>) CreateShipCountField(int size, CellState[,] field)
        {
            var shipCountField = new int[size, size];
            var ships = new Dictionary<int, int>();

            var number = 1;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    // pokud v bunce je nezaznamena lod, zaznamena ji.
                    if (field[i, j] == CellState.Ship && shipCountField[i, j] == 0)
                    {
                        InitShip(size, field, ships, shipCountField, i, j, number);

                        number++;
                    }
                }
            }

            return (shipCountField, ships);
        }

        /// <summary>
        /// Inicializuje lod na poli rekurzivne.
        /// </summary>
        /// <param name="size">Velikost pole.</param>
        /// <param name="field">Pole stavu bunek.</param>
        /// <param name="ships">Slovnik obsahujici cisla lodi a pocty.</param>
        /// <param name="shipCountField">Pole poctu lodi.</param>
        /// <param name="x">Souradnice x bunky.</param>
        /// <param name="y">Souradnice y bunky.</param>
        /// <param name="number">Cislo aktualni lode.</param>
        private static void InitShip(
            in int size, in CellState[,] field,
            in Dictionary<int, int> ships, in int[,] shipCountField,
            int x, int y, int number)
        {
            // ukonci, pokud souradnice jsou mimo pole.
            if (x < 0 || y < 0 || x >= size || y >= size)
                return;

            // ukonci, pokud v bunce neni lod anebo lod je jiz zaznamena.
            if (field[x, y] != CellState.Ship || shipCountField[x, y] != 0)
                return;

            // zaznameni
            shipCountField[x, y] = number;

            if (!ships.ContainsKey(number))
                ships[number] = 0;

            ships[number]++;

            // pokracuje v bunkach kolem tehle.
            InitShip(size, field, ships, shipCountField, x + 1, y, number);
            InitShip(size, field, ships, shipCountField, x - 1, y, number);
            InitShip(size, field, ships, shipCountField, x, y + 1, number);
            InitShip(size, field, ships, shipCountField, x, y - 1, number);
        }
    }

}

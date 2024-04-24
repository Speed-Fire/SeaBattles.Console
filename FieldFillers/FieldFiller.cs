using SeaBattles.Console.Misc;

#nullable disable

namespace SeaBattles.Console.FieldFillers
{
    /// <summary>
    /// Trida reprezentujici plneni pole lodemi.
    /// </summary>
    public class FieldFiller
    {
        private readonly CellState[,] _field;
        private readonly int _size;
        private readonly Dictionary<ShipSize, int> _availableShips;

        /// <summary>
        /// Dostupne lodě, které mohou být umístěny na poli.
        /// </summary>
        public IReadOnlyDictionary<ShipSize, int> AvailableShips => _availableShips;

        /// <summary>
        /// Velikost pole.
        /// </summary>
        public int Size => _size;

        /// <summary>
        /// Konstruktor tridy FieldFiller.
        /// </summary>
        /// <param name="size">Velikost pole.</param>
        /// <param name="availableShips">Dostupne lode pro umisteni na pole.</param>
        internal FieldFiller(int size, Dictionary<ShipSize, int> availableShips)
        {
            _field = new CellState[size, size];
            _size = size;

            _availableShips = availableShips;
        }

        /// <summary>
        /// Vytvori pole boje s lodemi.
        /// </summary>
        /// <returns>Bojove pole s lodemi.</returns>
        public BattleField Build()
        {
            var (shipCountField, ships) = FieldShipCalculator.CreateShipCountField(Size, _field);

            return new BattleField(_size, _field, shipCountField, ships);
        }

        /// <summary>
        /// Umisti lod na pole, pokud je umisteni mozne.
        /// </summary>
        /// <param name="leftCornerX">X-ova souradnice leveho horniho rohu lod.</param>
        /// <param name="leftCornerY">Y-ova souradnice leveho horniho rohu lod.</param>
        /// <param name="size">Velikost lod.</param>
        /// <param name="direction">Smer lod (horizontalni nebo vertikalni).</param>
        /// <returns>True, pokud byla lod uspesne umistena, jinak False.</returns>
        public bool PutShip(int leftCornerX, int leftCornerY, ShipSize size, ShipDirection direction)
        {
            if (!AvailableShips.ContainsKey(size) || AvailableShips[size] <= 0)
                return false;

            var length = (int)size;

            var dirX = 1;
            var dirY = 0;

            if (direction == ShipDirection.Horizontal)
                (dirX, dirY) = (dirY, dirX);

            if (!IsCellSuitableForShip(leftCornerX, leftCornerY, length, dirX, dirY))
                return false;

            for (int i = 0; i < length; i++)
            {
                var x = leftCornerX + i * dirX;
                var y = leftCornerY + i * dirY;

                _field[x, y] = CellState.Ship;
            }

            _availableShips[size]--;

            if (_availableShips[size] == 0)
            {
                _availableShips.Remove(size);
            }

            return true;
        }

        /// <summary>
        /// Kontroluje, zda je bunka vhodna pro umisteni lod.
        /// </summary>
        /// <param name="x">X-ova souradnice bunky.</param>
        /// <param name="y">Y-ova souradnice bunky.</param>
        /// <param name="length">Delka lod.</param>
        /// <param name="dirX">Smer pohybu na ose X.</param>
        /// <param name="dirY">Smer pohybu na ose Y.</param>
        /// <returns>True, pokud je bunka vhodna pro umisteni lod, jinak False.</returns>
        private bool IsCellSuitableForShip(int x, int y, int length, int dirX, int dirY)
        {
            var pairs = new (int, int)[] { (0, 0), (1, 1), (0, 1), (1, 0), (-1, -1), (0, -1), (-1, 0), (1, -1), (-1, 1) };

            for (int i = 0; i < length; i++)
            {
                var startX = x + i * dirX;
                var startY = y + i * dirY;

                foreach (var pair in pairs)
                {
                    var shiftedX = startX + pair.Item1;
                    var shiftedY = startY + pair.Item2;

                    var isXoutOfSize = IsOutOfSize(shiftedX);
                    var isYoutOfSize = IsOutOfSize(shiftedY);


                    if ((pair.Item1 != 0 && isXoutOfSize) ||
                        (pair.Item2 != 0 && isYoutOfSize))
                        continue;

                    if (isXoutOfSize || isYoutOfSize)
                        return false;

                    if (_field[shiftedX, shiftedY] != CellState.Empty)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Kontroluje, zda je souradnice mimo rozsah pole.
        /// </summary>
        /// <param name="pos">Souradnice.</param>
        /// <returns>True, pokud je souradnice mimo pole, jinak False.</returns>
        private bool IsOutOfSize(int pos) => pos < 0 || pos >= _size;

        /// <summary>
        /// Vykresli pole.
        /// </summary>
        public void Draw()
        {
            BattlefieldDrawer.Draw(_field, _size);
        }
    }

}

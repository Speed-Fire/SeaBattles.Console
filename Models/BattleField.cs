using System.Text;

namespace SeaBattles.Console
{
    /// <summary>
    /// Trida reprezentujici bitevni pole.
    /// </summary>
    public class BattleField
    {
        /// <summary>
        /// Pole obsahujici stav jednotlivych bunk na bitevnim poli.
        /// </summary>
        public CellState[,] Field { get; }

        /// <summary>
        /// Velikost bitevniho pole.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Pocet lodi na bitevnim poli.
        /// </summary>
        public int ShipCount => _ships.Count;

        /// <summary>
        /// Indikuje, zda je bitevni pole prazdne (neobsahuje zadne lode).
        /// </summary>
        public bool IsEmpty => ShipCount == 0;

        /// <summary>
        /// Pocet obsazenych lodemi bunek.
        /// </summary>
        private int ShipCellCount { get; set; }

        /// <summary>
        /// Pomocna mapa pro _shipCountField.
        /// </summary>
        private readonly Dictionary<int, int> _ships;

        /// <summary>
        /// Dodatecne pole pro rychle pocitani poctu plavbyschopnych lodi.
        /// </summary>
        private readonly int[,] _shipCountField;

        internal BattleField(int size, CellState[,] field, int[,] shipCountField, Dictionary<int, int> ships)
        {
            Field = field;

            _shipCountField = shipCountField;
            _ships = ships;

            Size = size;

            ShipCellCount = CalculateShipCellCount();
        }

        /// <summary>
        /// Indexer pro pristup k bunecnemu stavu na zaslaných souradnicích.
        /// </summary>
        public CellState this[int x, int y] => Field[x, y];

        /// <summary>
        /// Provede utok na zadanou bunku na bitevnim poli.
        /// </summary>
        internal AttackResult Attack(uint x, uint y)
        {
            if (x >= Size || y >= Size)
                throw new ArgumentOutOfRangeException();

            var shipCount = ShipCount;

            switch (Field[x, y])
            {
                case CellState.Empty:
                    Field[x, y] = CellState.Attacked;
                    return AttackResult.Missed;

                case CellState.Attacked:
                case CellState.Destroyed:
                    return AttackResult.Failed;

                case CellState.Ship:
                    Field[x, y] = CellState.Destroyed;
                    ShipCellCount--;

                    RecalculateShipCount(x, y);

                    return shipCount == ShipCount ? AttackResult.Hitten : AttackResult.Destroyed;
            }

            return AttackResult.Failed;
        }

        /// <summary>
        /// Vrati nahodnou bunku obsahujici lod.
        /// </summary>
        internal (int, int)? GetRandomShipCell()
        {
            if (ShipCellCount == 0)
                return null;

            var rand = new Random();
            var cellNumber = rand.Next(0, ShipCellCount);

            var iter = 0;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Field[i, j] == CellState.Ship)
                    {
                        if (iter++ == cellNumber)
                            return (i, j);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Prepocita pocet bunek obsahujicich lode po utoku na zaslane souradnice.
        /// </summary>
        private void RecalculateShipCount(uint x, uint y)
        {
            var number = _shipCountField[x, y];

            if (number == 0)
                return;

            _shipCountField[x, y] = 0;

            if (--_ships[number] <= 0)
                _ships.Remove(number);
        }

        /// <summary>
        /// Spocita celkovy pocet bunek obsahujicich lode.
        /// </summary>
        private int CalculateShipCellCount()
        {
            var res = 0;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Field[i, j] == CellState.Ship)
                        res++;
                }
            }

            return res;
        }

        /// <summary>
        /// Prevede stav bitevniho pole na retezec.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    var ch = (char)('0' + Field[i, j]);

                    sb.Append(ch);
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }

}

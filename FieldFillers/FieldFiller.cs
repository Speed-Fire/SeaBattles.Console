using SeaBattles.Console.Misc;

#nullable disable

namespace SeaBattles.Console.FieldFillers
{
    public class FieldFiller
    {
        private readonly CellState[,] _field;
        private readonly int _size;
        private readonly Dictionary<ShipSize, int> _availableShips;

        public IReadOnlyDictionary<ShipSize, int> AvailableShips => _availableShips;
        public int Size => _size;

        internal FieldFiller(int size, Dictionary<ShipSize, int> availableShips)
        {
            _field = new CellState[size, size];
            _size = size;

            _availableShips = availableShips;
        }

        public BattleField Build()
        {
            var (shipCountField, ships) = CreateShipCountField();

            return new BattleField(_size, _field, shipCountField, ships);
        }

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

        private bool IsOutOfSize(int pos) => pos < 0 || pos >= _size;

        public void Draw()
        {
            BattlefieldDrawer.Draw(_field, _size);
        }

		#region Building ship count field

		private (int[,], Dictionary<int,int>) CreateShipCountField()
		{
            var shipCountField = new int[Size, Size];
            var ships = new Dictionary<int, int>();

			var number = 1;

			for (int i = 0; i < Size; i++)
			{
				for (int j = 0; j < Size; j++)
				{
					if (_field[i, j] == CellState.Ship && shipCountField[i, j] == 0)
					{
						InitShip(ships, shipCountField, i, j, number);

						number++;
					}
				}
			}

            return (shipCountField, ships);
		}

		private void InitShip(in Dictionary<int, int> ships, in int[,] shipCountField,
            int x, int y, int number)
		{
			if (x < 0 || y < 0 || x >= Size || y >= Size)
				return;

			if (_field[x, y] != CellState.Ship || shipCountField[x, y] != 0)
				return;

			shipCountField[x, y] = number;

            if(!ships.ContainsKey(number))
                ships[number] = 0;

			ships[number]++;

			InitShip(ships, shipCountField, x + 1, y, number);
			InitShip(ships, shipCountField, x - 1, y, number);
			InitShip(ships, shipCountField, x, y + 1, number);
			InitShip(ships, shipCountField, x, y - 1, number);
		}

		#endregion
	}
}

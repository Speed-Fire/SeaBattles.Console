namespace SeaBattles.Console
{
    public class BattleField
	{
		public CellState[,] Field { get; }
		public int Size { get; }
		public int ShipCellCount { get; private set; } // count of cells with ships: a 4-long ship is 4 cells
		public int ShipCount => _ships.Count;
		public bool IsEmpty => //ShipCellCount == 0; 
			ShipCount == 0;

		private readonly Dictionary<int, int> _ships;
		private readonly int[,] _shipCountField;

		internal BattleField(int size, CellState[,] field,
			int[,] shipCountField, Dictionary<int, int> ships)
		{
			Field = field;
			
			_shipCountField = shipCountField;
			_ships = ships;

			Size = size;
		}

		public CellState this[int x, int y]
		{
			get => Field[x, y];
		}

		internal AttackResult Attack(uint x, uint y)
		{
			if (x >= Size || y >= Size)
				throw new ArgumentOutOfRangeException();

			var shipCount = ShipCount;

			switch(Field[x, y])
			{
				case CellState.Empty:
					Field[x, y] = CellState.Attacked;
					return AttackResult.Missed;

				case CellState.Attacked:
				case CellState.Destroyed:
				//case CellState.Unknown:
					return AttackResult.Failed;

				case CellState.Ship:
					Field[x, y] = CellState.Destroyed;
					ShipCellCount--;

					RecalculateShipCount(x, y);

					return shipCount == ShipCount ? AttackResult.Hitten : AttackResult.Destroyed;
			}

			return AttackResult.Failed;
		}

		private void RecalculateShipCount(uint x, uint y)
		{
			var number = _shipCountField[x, y];

			if (number == 0)
				return;

			_shipCountField[x, y] = 0;

			if (--_ships[number] <= 0)
				_ships.Remove(number);
		}
	}
}

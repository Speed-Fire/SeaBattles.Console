using SeaBattles.Console.FieldFillers;
using System.Runtime.Serialization;
using System.Text;

namespace SeaBattles.Console
{
	[DataContract]
    public class BattleField
	{
		[DataMember]
		public CellState[,] Field { get; }

		[DataMember]
		public int Size { get; }

		public int ShipCount => _ships.Count;
		public bool IsEmpty => //ShipCellCount == 0; 
			ShipCount == 0;

		private int ShipCellCount { get; set; } // count of cells with ships: a 4-long ship is 4 cells

		private Dictionary<int, int> _ships;
		private int[,] _shipCountField;

		internal BattleField(int size, CellState[,] field,
			int[,] shipCountField, Dictionary<int, int> ships)
		{
			Field = field;
			
			_shipCountField = shipCountField;
			_ships = ships;

			Size = size;

			ShipCellCount = CalculateShipCellCount();
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

		private void RecalculateShipCount(uint x, uint y)
		{
			var number = _shipCountField[x, y];

			if (number == 0)
				return;

			_shipCountField[x, y] = 0;

			if (--_ships[number] <= 0)
				_ships.Remove(number);
		}

		private int CalculateShipCellCount()
		{
			var res = 0;

			for(int i = 0; i < Size; i++)
			{
				for(int j = 0; j < Size; j++)
				{
					if (Field[i, j] == CellState.Ship)
						res++;
				}
			}

			return res;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			for(int i = 0; i < Size; i++)
			{
				for(int j = 0; j < Size; j++)
				{
					var ch = (char)('0' + Field[i, j]);

					sb.Append(ch);
				}

				sb.Append(Environment.NewLine);
			}

			return sb.ToString();
		}

		[OnDeserialized]
		private void InitShipCountField()
		{
			var (field, dict) = FieldShipCalculator.CreateShipCountField(Size, Field);

			_shipCountField = field;
			_ships = dict;

			ShipCellCount = CalculateShipCellCount();
		}
	}
}

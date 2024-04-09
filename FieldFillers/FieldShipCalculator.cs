namespace SeaBattles.Console.FieldFillers
{
	internal class FieldShipCalculator
	{
		public static (int[,], Dictionary<int, int>) CreateShipCountField(int size, CellState[,] field)
		{
			var shipCountField = new int[size, size];
			var ships = new Dictionary<int, int>();

			var number = 1;

			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					if (field[i, j] == CellState.Ship && shipCountField[i, j] == 0)
					{
						InitShip(size, field, ships, shipCountField, i, j, number);

						number++;
					}
				}
			}

			return (shipCountField, ships);
		}

		private static void InitShip(
			in int size, in CellState[,] field,
			in Dictionary<int, int> ships, in int[,] shipCountField,
			int x, int y, int number)
		{
			if (x < 0 || y < 0 || x >= size || y >= size)
				return;

			if (field[x, y] != CellState.Ship || shipCountField[x, y] != 0)
				return;

			shipCountField[x, y] = number;

			if (!ships.ContainsKey(number))
				ships[number] = 0;

			ships[number]++;

			InitShip(size, field, ships, shipCountField, x + 1, y, number);
			InitShip(size, field, ships, shipCountField, x - 1, y, number);
			InitShip(size, field, ships, shipCountField, x, y + 1, number);
			InitShip(size, field, ships, shipCountField, x, y - 1, number);
		}
	}
}

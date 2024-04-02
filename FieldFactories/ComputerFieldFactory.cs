using SeaBattles.Console.FieldFillers;
using SeaBattles.Console.Misc;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.FieldFactories
{
    internal class ComputerFieldFactory : IFieldFactory
	{
		private const int MAX_BAD_SHIP_PLACING = 3;

		private readonly Random _random = new();

		public BattleField CreateBattlefield(FieldSetup setup)
		{
			while (true)
			{
				var badIter = 0;

				var filler = FieldFillerFactory.Create(setup.Size);

				while (filler.AvailableShips.Any())
				{
					var (x, y) = ChooseCoords(setup.Size);

					var size = ChooseSize(filler);

					var direction = ChooseDirection();

					if (!filler.PutShip(x, y, size, direction))
						badIter++;

					if (badIter == MAX_BAD_SHIP_PLACING)
						break;
				}

				if (!filler.AvailableShips.Any())
					return filler.Build();
			}
		}

		private ShipDirection ChooseDirection()
		{
			return (ShipDirection)_random.Next(0, 2);
		}

		private ShipSize ChooseSize(FieldFiller filler)
		{
			var availableShips = filler.AvailableShips;

			var keys = availableShips.Keys;

			var pos = _random.Next(0, keys.Count());

			return keys.ElementAt(pos);
		}

		private static (int, int) ChooseCoords(int maxSize)
		{
			var res = ComputerFieldCoordsGenerator.Generate(maxSize);

			return (res.X, res.Y);
		}
	}
}

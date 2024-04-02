using SeaBattles.Console.FieldFillers;
using SeaBattles.Console.Misc;
using SeaBattles.Console.Models;

#nullable disable

namespace SeaBattles.Console.FieldFactories
{
    internal class UserFieldFactory : IFieldFactory
    {
        public BattleField CreateBattlefield(FieldSetup setup)
        {
            Start:

            var filler = FieldFillerFactory.Create(setup.Size);

			while(true)
			{
				Draw(filler);

				if (!filler.AvailableShips.Any())
				{
					if (!AskToContinue())
						goto Start;

					return filler.Build();
				}

				var str = System.Console.ReadLine();

				if (str.Equals("smaz"))
					goto Start;

				if (ShipCoordinateParser.TryParse(str, filler.Size, out var x, out var y,
					out var direction, out var shipSize))
				{
					filler.PutShip(x, y, shipSize, direction);
				}
			}
        }

		private static void Draw(FieldFiller filler)
		{
			System.Console.Clear();

			System.Console.WriteLine($"Postupne zadavejte pozice leveho" +
				$" horniho koutu vase lodi, jeji smer " +
				$"a rozmer:");

			System.Console.WriteLine();

			System.Console.WriteLine("Dostupne smery:");
			System.Console.WriteLine($"   Svisla ({ShipCoordinateParser.CHAR_VERTICAL_DIRECTION})");
			System.Console.WriteLine($"   Vodorovna ({ShipCoordinateParser.CHAR_HORIZONTAL_DIRECTION})");

			System.Console.WriteLine();

			System.Console.WriteLine("Dostupne lode:");
			PrintAvailableShips(filler);
			System.Console.WriteLine();

			System.Console.WriteLine($"Priklad: a1 {ShipCoordinateParser.CHAR_VERTICAL_DIRECTION}" +
				$" {ShipCoordinateParser.CHAR_SHIP_LARGE}");

			System.Console.WriteLine();
			filler.Draw();
			System.Console.WriteLine();

			System.Console.WriteLine("Pokud chcete vycistit plochu, zadejte \'smaz\'.");
			System.Console.WriteLine();
		}

		private static void PrintAvailableShips(FieldFiller filler)
        {
            System.Console.Write("   ");


			foreach (var pair in filler.AvailableShips)
            {
                switch (pair.Key)
                {
                    case ShipSize.Tiny:
                        System.Console.Write($"Maly ({ShipCoordinateParser.CHAR_SHIP_TINY}): {pair.Value};  ");
                        break;
                    case ShipSize.Small:
                        System.Console.Write($"Stredni ({ShipCoordinateParser.CHAR_SHIP_SMALL}): {pair.Value};  ");
                        break;
                    case ShipSize.Medium:
                        System.Console.Write($"Velky ({ShipCoordinateParser.CHAR_SHIP_MEDIUM}): {pair.Value};  ");
                        break;
                    case ShipSize.Large:
                        System.Console.Write($"Obrovsky ({ShipCoordinateParser.CHAR_SHIP_LARGE}): {pair.Value};  ");
                        break;
                }
            }

            System.Console.WriteLine();
        }

		private static bool AskToContinue()
		{
			System.Console.WriteLine();
			System.Console.WriteLine("Pokracujte stiskem cokoliv jineho.");

			var str = System.Console.ReadLine();

			if (str.Equals("smaz"))
				return false;

			return true;
		}
    }
}

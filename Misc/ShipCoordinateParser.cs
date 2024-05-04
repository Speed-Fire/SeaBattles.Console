namespace SeaBattles.Console.Misc
{
	/// <summary>
	/// Trida pro parsovani souradnic lodi.
	/// </summary>
	internal static class ShipCoordinateParser
    {
        public const char CHAR_HORIZONTAL_DIRECTION = 'v';
        public const char CHAR_VERTICAL_DIRECTION = 's';

        public const char CHAR_SHIP_TINY = 'm';
        public const char CHAR_SHIP_SMALL = 's';
        public const char CHAR_SHIP_MEDIUM = 'v';
        public const char CHAR_SHIP_LARGE = 'o';

		/// <summary>
		/// Metoda pro pokus o parsovani souradnic.
		/// Format vstupu: pismeno cislo(1 az dva cislice)
		/// </summary>
		/// <param name="input">Vstupni retezec obsahujici souradnice.</param>
		/// <param name="maxsize">Maximalni velikost hraciho pole.</param>
		/// <param name="x">Vystupni hodnota pro x souradnici.</param>
		/// <param name="y">Vystupni hodnota pro y souradnici.</param>
		/// <returns>True, pokud byly souradnice uspesne nacteny; jinak false.</returns>
		internal static bool TryParse(string input, int maxsize, out int x, out int y)
		{
			x = -1;
			y = -1;

			input = input.Replace(" ", string.Empty);

			if (input.Length < 2 || input.Length > 3)
				return false;

			// kontrola y souradnice
			var tmpY = char.ToLower(input[0]) - 'a';
			if (tmpY < 0 || tmpY > maxsize)
				return false;

			// kontrola x souradnice
			if (!int.TryParse(input[1..], out var tmpX))
				return false;

			if (tmpX < 1 || tmpX > maxsize)
				return false;

			tmpX--;

			// prirazeni hodnot pokud je vse v poradku
			x = tmpY;
			y = tmpX;

			return true;
		}

		/// <summary>
		/// Metoda pro pokus o parsovani souradnic, smeru a velikosti lode.
		/// Format vstupu: pismeno cislo(1 az dva cislice) smer velikost
		/// </summary>
		/// <param name="input">Vstupni retezec obsahujici souradnice, smer a velikost lode.</param>
		/// <param name="maxsize">Maximalni velikost hraciho pole.</param>
		/// <param name="x">Vystupni hodnota pro x souradnici.</param>
		/// <param name="y">Vystupni hodnota pro y souradnici.</param>
		/// <param name="direction">Vystupni hodnota pro smer lode.</param>
		/// <param name="size">Vystupni hodnota pro velikost lode.</param>
		/// <returns>True, pokud byly souradnice a informace o lode uspesne nacteny; jinak false.</returns>
		internal static bool TryParse(string input, int maxsize, out int x, out int y,
			out ShipDirection direction, out ShipSize size)
		{
			x = -1;
			y = -1;
			direction = 0;
			size = 0;

			input = input.Replace(" ", string.Empty);

			if (input.Length < 4 || input.Length > 5)
				return false;

			// kontrola y souradnice
			var tmpY = char.ToLower(input[0]) - 'a';
			if (tmpY < 0 || tmpY >= maxsize)
				return false;

			// kontrola x souradnice
			if (!int.TryParse(input.AsSpan(1, input.Length == 5 ? 2 : 1), out var tmpX))
				return false;

			if (tmpX < 1 || tmpX > maxsize)
				return false;

			tmpX--;

			// kontrola smeru
			var lastChar = char.ToLower(input[input.Length == 5 ? 3 : 2]);
			ShipDirection tmpDir;

			if (lastChar == CHAR_HORIZONTAL_DIRECTION)
			{
				tmpDir = ShipDirection.Horizontal;
			}
			else if (lastChar == CHAR_VERTICAL_DIRECTION)
			{
				tmpDir = ShipDirection.Vertical;
			}
			else
				return false;

			// kontrola velikosti
			ShipSize tmpSize;

			switch (char.ToLower(input.Last()))
			{
				case CHAR_SHIP_TINY:
					tmpSize = ShipSize.Tiny;
					break;
				case CHAR_SHIP_SMALL:
					tmpSize = ShipSize.Small;
					break;
				case CHAR_SHIP_MEDIUM:
					tmpSize = ShipSize.Medium;
					break;
				case CHAR_SHIP_LARGE:
					tmpSize = ShipSize.Large;
					break;
				default:
					return false;
			}

			// prirazeni ziskanych hodnot pokud je vse v poradku
			y = tmpX;
			x = tmpY;

			direction = tmpDir;
			size = tmpSize;

			return true;
		}
	}
}

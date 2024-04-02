namespace SeaBattles.Console.Misc
{
    internal static class ShipCoordinateParser
    {
        public const char CHAR_HORIZONTAL_DIRECTION = 'v';
        public const char CHAR_VERTICAL_DIRECTION = 's';

        public const char CHAR_SHIP_TINY = 't';
        public const char CHAR_SHIP_SMALL = 's';
        public const char CHAR_SHIP_MEDIUM = 'm';
        public const char CHAR_SHIP_LARGE = 'l';

        internal static bool TryParse(string input, int maxsize, out int x, out int y)
        {
            x = -1;
            y = -1;

            input = input.Replace(" ", string.Empty);

            if (input.Length < 2 || input.Length > 3)
                return false;

            // check y coordinate
            var tmpY = char.ToLower(input[0]) - 'a';
            if (tmpY < 0 || tmpY > maxsize)
                return false;

            // check x coordinate
            if (!int.TryParse(input[1..], out var tmpX))
                return false;

			if (tmpX < 1 || tmpX > maxsize)
				return false;

            tmpX--;

            // assign values if all is fine
            x = tmpY;
            y = tmpX;

            return true;
        }

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

            // check y coordinate
            var tmpY = char.ToLower(input[0]) - 'a';
            if (tmpY < 0 || tmpY >= maxsize)
                return false;

            // check x coordinate
            if (!int.TryParse(input.AsSpan(1, input.Length == 5 ? 2 : 1), out var tmpX))
                return false;

            if(tmpX < 1 || tmpX > maxsize)
                return false;

            tmpX--;

            // check direction
            var lastChar = char.ToLower(input[input.Length == 5 ? 3 : 2]);
            ShipDirection tmpDir = 0;

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

            // check size
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


            // assign gotten values if all is fine
            y = tmpX;
            x = tmpY;

            direction = tmpDir;
            size = tmpSize;

            return true;
        }
    }
}

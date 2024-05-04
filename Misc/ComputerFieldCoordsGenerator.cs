namespace SeaBattles.Console.Misc
{
    /// <summary>
    /// Struktura pro uchovani souradnic.
    /// </summary>
    public record Coords(int X, int Y);

    /// <summary>
    /// Vyctovy typ pro urceni kvadrantu.
    /// </summary>
    public enum Quadrant : int
    {
        FIRST,
        SECOND,
        THIRD,
        FOURTH
    }

    /// <summary>
    /// Staticka trida pro generovani souradnic pro pocitacove pole.
    /// </summary>
    internal static class ComputerFieldCoordsGenerator
    {
        /// <summary>
        /// Generuje souradnice na zaklade velikosti pole.
        /// </summary>
        /// <param name="fieldSize">Velikost pole.</param>
        /// <returns>Souradnice.</returns>
        public static Coords Generate(int fieldSize)
        {
            var quadrant = GetQuadrant();

            var halfSize = fieldSize / 2;

            var x = 0;
            var y = 0;

            switch (quadrant)
            {
                case Quadrant.FIRST:
                    break;
                case Quadrant.SECOND:
                    x = halfSize;
                    break;
                case Quadrant.THIRD:
                    y = halfSize;
                    break;
                case Quadrant.FOURTH:
                    x = halfSize;
                    y = halfSize;
                    break;
            }

            var rand = new Random();

            x += rand.Next(0, fieldSize - halfSize);
            y += rand.Next(0, fieldSize - halfSize);

            if (x >= fieldSize)
                x = fieldSize - 1;

            if (y >= fieldSize)
                y = fieldSize - 1;

            return new Coords(x, y);
        }

        /// <summary>
        /// Ziska nahodny kvadrant.
        /// </summary>
        /// <returns>Kvadrant.</returns>
        private static Quadrant GetQuadrant()
        {
            var rand = new Random();

            return (Quadrant)rand.Next(0, 4);
        }
    }
}

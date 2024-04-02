using SeaBattles.Console;

namespace SeaBattles.Console.FieldFillers
{
    internal static class FieldFillerFactory
    {
        // Bulgarian values
        private const decimal LARGE_PERCENTAGE = 0.098408221571475470841577262m;
        private const decimal MEDIUM_PERCENTAGE = 0.1787996735411186080588221623m;
        private const decimal SMALL_PERCENTAGE = 0.2633190062237051900115105355m;
        private const decimal TINY_PERCENTAGE = 0.4594730986637007310880900402m;


        public static FieldFiller Create(int fieldSize)
        {
            var shipCount = CalculateShipCount(fieldSize);

            var dict = new Dictionary<ShipSize, int>
            {
                [ShipSize.Large] = (int)Math.Round(shipCount * LARGE_PERCENTAGE),
                [ShipSize.Medium] = (int)Math.Round(shipCount * MEDIUM_PERCENTAGE),
                [ShipSize.Small] = (int)Math.Round(shipCount * SMALL_PERCENTAGE),
                [ShipSize.Tiny] = (int)Math.Round(shipCount * TINY_PERCENTAGE)
            };

            return new FieldFiller(fieldSize, dict);
        }

        // Bulgarian calculation
        private static int CalculateShipCount(int fieldSize)
        {
            if (fieldSize < 10)
                return fieldSize;

            var states = new int[] { 2, 2, 3, 3, 5 };

            var addition = 0;

            for (int i = 10 - 1, state = 0; i < fieldSize; i++, state = (state + 1) % states.Length)
            {
                addition += states[state];
            }

            return fieldSize + addition;
        }
    }
}

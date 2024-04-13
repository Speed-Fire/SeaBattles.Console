namespace SeaBattles.Console.Misc
{
    internal static class BattlefieldDrawer
    {
        private const char CELL_EMPTY = '∙';
        private const char CELL_SHIP = '■';
        private const char CELL_ATTACKED = '҉';
        private const char CELL_DESTROYED = '†';

        internal static void Draw(BattleField field, bool hideShips = false, IReadOnlyList<(int,int)>? hints = null)
        {
            Draw(field.Field, field.Size, hideShips, hints);
        }

        internal static void Draw(CellState[,] field, int size, bool hideShips = false,
            IReadOnlyList<(int, int)>? hints = null)
        {
            System.Console.Write("  ");
            for (int i = 0; i < size; i++)
            {
                System.Console.Write((char)('①' + i));
            }
            System.Console.WriteLine();

            var color = System.Console.ForegroundColor;

			for (int i = 0; i < size; i++)
            {
                System.Console.Write((char)('a' + i));
                System.Console.Write(' ');

                for (int j = 0; j < size; j++)
                {
                    switch (field[i, j])
                    {
                        case CellState.Empty:
                            System.Console.Write(CELL_EMPTY);
                            break;
                        case CellState.Ship:
							if (hideShips && (hints is null || !hints.Contains((i, j))))
                            {
								System.Console.Write(CELL_EMPTY);
							}
                            else
                            {
								if (hints is not null && hints.Contains((i, j)))
                                    System.Console.ForegroundColor = ConsoleColor.DarkYellow;

								System.Console.Write(CELL_SHIP);

                                System.Console.ForegroundColor = color;
							}

                            break;
                        case CellState.Attacked:
                            System.Console.ForegroundColor = ConsoleColor.Cyan;

							System.Console.Write(CELL_ATTACKED);

							System.Console.ForegroundColor = color;
							break;
                        case CellState.Destroyed:
							System.Console.ForegroundColor = ConsoleColor.DarkRed;

							System.Console.Write(CELL_DESTROYED);

							System.Console.ForegroundColor = color;
							break;
                    }
                }

                System.Console.WriteLine();
            }
        }
    }
}

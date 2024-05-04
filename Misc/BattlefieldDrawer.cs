﻿namespace SeaBattles.Console.Misc
{
	/// <summary>
	/// Trida pro vykreslovani hraciho pole.
	/// </summary>
	internal static class BattlefieldDrawer
    {
        //private const char CELL_EMPTY = '∙';
        private const char CELL_EMPTY = '░';
        private const char CELL_SHIP = '■';
        private const char CELL_ATTACKED = '҉';
        private const char CELL_DESTROYED = '†';

		/// <summary>
		/// Vykresli hraci pole.
		/// </summary>
		/// <param name="field">Hraci pole k vykresleni.</param>
		/// <param name="hideShips">Urcuje, zda maji byt lode skryty.</param>
		/// <param name="hints">Seznam napovedi, pokud jsou k dispozici.</param>
		internal static void Draw(BattleField field, bool hideShips = false, IReadOnlyList<(int,int)>? hints = null)
        {
            Draw(field.Field, field.Size, hideShips, hints);
        }

		/// <summary>
		/// Vykresli hraci pole na zaklade zadanych dat.
		/// </summary>
		/// <param name="field">Hraci pole k vykresleni.</param>
		/// <param name="size">Velikost hraciho pole.</param>
		/// <param name="hideShips">Urcuje, zda maji byt lode skryty.</param>
		/// <param name="hints">Seznam napovedi, pokud jsou k dispozici.</param>
		internal static void Draw(CellState[,] field, int size, bool hideShips = false,
            IReadOnlyList<(int, int)>? hints = null)
        {
            // vypise vodorvna cisla bunek.
            System.Console.Write("  ");
            for (int i = 0; i < size; i++)
            {
                System.Console.Write((char)('①' + i));
            }
            System.Console.WriteLine();

            var color = System.Console.ForegroundColor;

			for (int i = 0; i < size; i++)
            {
                // vypise svisla cisla(zde jiz pismena) bunek.
                System.Console.Write((char)('a' + i));
                System.Console.Write(' ');

                // vypise obsah pole
                for (int j = 0; j < size; j++)
                {
                    switch (field[i, j])
                    {
                        // prazdna bunka.
                        case CellState.Empty:
                            System.Console.Write(CELL_EMPTY);
                            break;

                        // obsahujici lod bunka.
                        case CellState.Ship:
                            // pokud zvoleno ukryti lodi, a neexistuje zadna napoveda
                            //  k teto bunce, zobrzi bunku jako prazdnou.
							if (hideShips && (hints is null || !hints.Contains((i, j))))
                            {
								System.Console.Write(CELL_EMPTY);
							}
                            // jinak zobrazi skutecny obsah bunky.
                            else
                            {
                                // pokud zobrazovani obsahu je diky napovedi,
                                //  pak zmeni barvu na zlutou.
								if (hints is not null && hints.Contains((i, j)))
                                    System.Console.ForegroundColor = ConsoleColor.DarkYellow;

								System.Console.Write(CELL_SHIP);

                                System.Console.ForegroundColor = color;
							}

                            break;

                        // utocena bunka.
                        case CellState.Attacked:
                            System.Console.ForegroundColor = ConsoleColor.Cyan;

							System.Console.Write(CELL_ATTACKED);

							System.Console.ForegroundColor = color;
							break;

                        // bunka se znicenou lode.
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

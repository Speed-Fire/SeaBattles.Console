using SeaBattles.Console.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattles.Console.States.Menus
{
	/// <summary>
	/// Trida reprezentujici stav prohlizeni praviel.
	/// </summary>
	internal class RulesState : UserInputState<Game>
	{
		private const string REGEX_CONTINUE = "^.*$";

		public RulesState(Game stateMachine) : base(stateMachine)
		{
		}

		/// <summary>
		/// Vykresli pravidla.
		/// </summary>
		protected override void Draw()
		{
			System.Console.Clear();

            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");
            System.Console.WriteLine("|     Pravidla     |");
            System.Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");

			System.Console.WriteLine();
			System.Console.WriteLine();

			PrintBlock("1. Cil hry: ");
			System.Console.WriteLine("Cilem hry je potopit vsechny lode soupere predtim, nez on potopi vase lode.");
			System.Console.WriteLine();

			PrintBlock("2. Priprava: ");
			System.Console.WriteLine("Hrac zvoli velikost pole od 7 do 15, potom obtiznost AI");
			System.Console.WriteLine("              a dal bude rozmistovat lode na pole. Jsou lode o 1, 2, 3 a 4 bunky.");
			System.Console.WriteLine("              Lode nelze umistovat diagonalne anebo prekryvat.");
			System.Console.WriteLine();

			PrintBlock("3. Tahy: ");
			System.Console.WriteLine("Hrac a pocitac se stridaji v tazich. Kazdy tah hrac zada souradnice bunky");
			System.Console.WriteLine("            na souperovem poli, na ktere chce zasahnout. Pokud hrac zasahne lod, pak");
			System.Console.WriteLine("            muze pokracovat dal ve strelbe. To same plati i pro pocitac.");
			System.Console.WriteLine();

			PrintBlock("4. Napovedi: ");
			System.Console.WriteLine("Hrac muze vyuzit napoved behem sveho tahu.");
			System.Console.WriteLine("              Maximalni poveleny pocet napovedi: 3.");
			System.Console.WriteLine();

			PrintBlock("5. Vitezstvi: ");
			System.Console.WriteLine("Hra pokracuje, dokud jeden z hracu nepotopi vsechny lode soupere.");
			System.Console.WriteLine("               Hrac, ktery potopi vsechny lode soupere, vyhrava hru.");
			System.Console.WriteLine();

			System.Console.WriteLine();
			System.Console.WriteLine();
			System.Console.WriteLine("Pokracujte do hlavniho menu stiskem libovolne klavesy...");
		}

		/// <summary>
		/// Vykresli predany <paramref name="msg"/> modrou barvou.
		/// </summary>
		/// <param name="msg"></param>
		private static void PrintBlock(string msg)
		{
			var originalColor = System.Console.ForegroundColor;

			System.Console.ForegroundColor = ConsoleColor.Cyan;
			System.Console.Write(msg);

			System.Console.ForegroundColor = originalColor;
		}

		/// <summary>
		/// Metoda pro inicializaci zpracovani vstupu pro stav s pravidlami.
		/// </summary>
		/// <param name="inputHandler">Instance tridy InputHandler pro registraci tokenu.</param>
		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add(REGEX_CONTINUE, _ => { SetState(new MainMenuState(StateMachine)); });
		}
	}
}

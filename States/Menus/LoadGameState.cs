using SeaBattles.Console.Input;
using SeaBattles.Console.Level;

namespace SeaBattles.Console.States.Menus
{
	/// <summary>
	/// Trida reprezentujici uloziste zaznamu her.
	/// </summary>
	internal class LoadGameState : UserInputState<Game>
	{
		private const string REGEX_EXIT = "^konc$";
		private const string REGEX_LOAD = @"^\d+$";
		private const string REGEX_REMOVE = @"^rem +\d+$";

		private const string MSG_SAVE_CORRUPTED = "zaznam je neplatny";
		private const string MSG_SAVE_REMOVE_SUCCESS = "zaznam je uspesne smazan";
		private const string MSG_SAVE_REMOVE_FAIL = "nepodarilo se smazat zaznam";

		private readonly List<LevelSave> _saves;

		public LoadGameState(Game game) : base(game)
		{
			_saves = new(LevelSaver.GetSaves());
		}

		#region Drawing

		/// <summary>
		/// Metoda pro vykresleni uloziste.
		/// </summary>
		protected override void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine("  Zadejte cislo zaznamu.");
			System.Console.WriteLine("  Pro smazani zadejte \"rem \" a cislo zaznamu (rem 1).");
			System.Console.WriteLine();
			System.Console.WriteLine(" Pokud se chcete vratit, zadejte \"konc\".");

			System.Console.WriteLine();
			System.Console.WriteLine();
			System.Console.WriteLine("  cislo  |      vytvoreno     |  velikost  |    UI    ");
			System.Console.WriteLine("---------+--------------------+----------+----------");

			// vypise vscheny pripustne zaznamy her.
			var i = 1;
			foreach(var save in _saves)
			{
				var (aiMind, mindColor) = save.Header.Difficulty switch
				{
					Models.Difficulty.Easy =>   ("Hloupy", ConsoleColor.DarkGreen),
					Models.Difficulty.Normal => ("Chytry", ConsoleColor.DarkYellow),
					Models.Difficulty.Hard =>   ("Moudry", ConsoleColor.DarkRed),
					_ => throw new Exception()
				};

				System.Console.Write($"    {i++}    |  {save.Created:dd.MM.yyyy HH:mm}  |     {save.Header.Size,-2}     |  ");

				var color = System.Console.ForegroundColor;
				System.Console.ForegroundColor = mindColor;

				System.Console.WriteLine($"{aiMind}  ");

				System.Console.ForegroundColor = color;
			}

			System.Console.WriteLine();
		}

		#endregion

		#region Input handlers

		/// <summary>
		/// Pokusi nahrat zaznam.
		/// </summary>
		/// <param name="input"></param>
		private void Load(string input)
		{
			// parsovani a kontrola vstupu.
			var number = int.Parse(input) - 1;

			if(number >= _saves.Count)
			{
				StateMsg = MSG_BAD_INPUT;
				return;
			}

			// pokus nahrani zaznamu.
			var save = _saves[number];

			var data = LevelSaver.Load(save);

			// pokud zaznam je poskozeny, nahlasi chybu.
			if(data is null)
			{
				StateMsg = MSG_SAVE_CORRUPTED;

				return;
			}

			// prevede automat na stav hry.
			var engine = new Console.Engine(data);

			SetState(new PlayingState(StateMachine, engine));
		}

		/// <summary>
		/// Smazat zaznam.
		/// </summary>
		/// <param name="input"></param>
		private void Remove(string input)
		{
			// parsovani a kontrola vstupu.
			var number = int.Parse(input.Substring(3).Trim()) - 1;

			if (number >= _saves.Count)
			{
				StateMsg = MSG_BAD_INPUT;
				return;
			}

			var save = _saves[number];

			// pokusi smazat zaznam.
			if (LevelSaver.Remove(save))
			{
				StateMsg = MSG_SAVE_REMOVE_SUCCESS;

				_saves.Remove(save);
			}
			// nahlasi chybu, pokud nastane neco spatne.
			else
			{
				StateMsg = MSG_SAVE_REMOVE_FAIL;
			}
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Metoda pro inicializaci zpracovani vstupu pro uloziste zaznamu.
		/// </summary>
		/// <param name="inputHandler">Instance tridy InputHandler pro registraci tokenu.</param>
		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add(REGEX_EXIT, (_) => { SetState(new MainMenuState(StateMachine)); });
			inputHandler.Add(REGEX_LOAD, Load);
			inputHandler.Add(REGEX_REMOVE, Remove);
		}

		#endregion
	}
}

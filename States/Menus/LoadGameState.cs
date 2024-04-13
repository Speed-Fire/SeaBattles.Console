using SeaBattles.Console.Input;
using SeaBattles.Console.Level;

namespace SeaBattles.Console.States.Menus
{
	internal class LoadGameState : UserInputState<Game>
	{
		private const string REGEX_EXIT = "^konc$";
		private const string REGEX_LOAD = @"^\d+$";
		private const string REGEX_REMOVE = @"^rem +\d+$";

		private const string MSG_SAVE_CORRUPTED = "ulozeni je neplatne";
		private const string MSG_SAVE_REMOVE_SUCCESS = "ulozeni je uspesne odstraneno";
		private const string MSG_SAVE_REMOVE_FAIL = "nepodarilo se odstranit ulozeni";

		private readonly List<LevelSave> _saves;

		public LoadGameState(Game game) : base(game)
		{
			_saves = new(LevelSaver.GetSaves());
		}

		#region Drawing

		protected override void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine("  Zadejte cislo ulozeni.");
			System.Console.WriteLine("  Pro odstraneni zadejte \"rem \" a cislo ulozeni (rem 1).");
			System.Console.WriteLine();
			System.Console.WriteLine(" Pokud se chcete vratit, zadejte \"konc\".");

			System.Console.WriteLine();
			System.Console.WriteLine();
			System.Console.WriteLine("  cislo  |      vytvoreno     |  rozmer  |    UI    ");
			System.Console.WriteLine("---------+--------------------+----------+----------");

			var i = 1;
			foreach(var save in _saves)
			{
				var (aiMind, mindColor) = save.Header.Difficulty switch
				{
					Models.Difficult.Easy =>   ("Hloupy", ConsoleColor.DarkGreen),
					Models.Difficult.Normal => ("Chytry", ConsoleColor.DarkYellow),
					Models.Difficult.Hard =>   ("Moudry", ConsoleColor.DarkRed),
					_ => throw new Exception()
				};

				System.Console.Write($"    {i++}    |  {save.Creaated:dd.MM.yyyy HH:mm}  |    {save.Header.Size,-2}    |  ");

				var color = System.Console.ForegroundColor;
				System.Console.ForegroundColor = mindColor;

				System.Console.WriteLine($"{aiMind}  ");

				System.Console.ForegroundColor = color;
			}

			System.Console.WriteLine();
		}

		#endregion

		#region Input handlers

		private void Load(string input)
		{
			var number = int.Parse(input) - 1;

			if(number >= _saves.Count)
			{
				StateMsg = MSG_BAD_INPUT;
				return;
			}

			var save = _saves[number];

			var data = LevelSaver.Load(save);

			if(data is null)
			{
				StateMsg = MSG_SAVE_CORRUPTED;

				return;
			}

			var engine = new Console.Engine(data);

			SetState(new PlayingState(StateMachine, engine));
		}

		private void Remove(string input)
		{
			var number = int.Parse(input.Substring(3).Trim()) - 1;

			if (number >= _saves.Count)
			{
				StateMsg = MSG_BAD_INPUT;
				return;
			}

			var save = _saves[number];

			if (LevelSaver.Remove(save))
			{
				StateMsg = MSG_SAVE_REMOVE_SUCCESS;

				_saves.Remove(save);
			}
			else
			{
				StateMsg = MSG_SAVE_REMOVE_FAIL;
			}
		}

		#endregion

		#region Initialization

		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add(REGEX_EXIT, (_) => { SetState(new MainMenuState(StateMachine)); });
			inputHandler.Add(REGEX_LOAD, Load);
			inputHandler.Add(REGEX_REMOVE, Remove);
		}

		#endregion
	}
}

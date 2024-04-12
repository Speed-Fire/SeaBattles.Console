using SeaBattles.Console.Input;
using SeaBattles.Console.Level;

namespace SeaBattles.Console.States.Menus
{
	internal class LoadGameState : IState
	{
		private const string REGEX_EXIT = "^konc$";
		private const string REGEX_LOAD = @"^\d+$";
		private const string REGEX_REMOVE = @"^rem +\d+$";

		private const string MSG_SAVE_CORRUPTED = "ulozeni je neplatne";
		private const string MSG_SAVE_REMOVE_SUCCESS = "ulozeni je uspesne odstraneno";
		private const string MSG_SAVE_REMOVE_FAIL = "nepodarilo se odstranit ulozeni";

		private readonly Game _game;
		private readonly InputHandler _inputHandler;

		private readonly List<LevelSave> _saves;

		public LoadGameState(Game game)
		{
			_game = game;

			_inputHandler = new();

			InitInputHandler();

			_saves = new(LevelSaver.GetSaves());
		}

		public void Invoke()
		{
			Draw();

			var input = (System.Console.ReadLine() ?? string.Empty).Trim();

			if (!_inputHandler.Handle(input))
			{
				_game.StateMsg = Console.Game.MSG_BAD_INPUT;
			}
		}

		#region Drawing

		private void Draw()
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
				var aiMind = save.Header.Difficulty switch
				{
					Models.Difficult.Easy =>   "Hloupy",
					Models.Difficult.Normal => "Chytry",
					Models.Difficult.Hard =>   "Moudry",
					_ => throw new Exception()
				};

				System.Console.WriteLine($"    {i++}    |  {save.Creaated:dd.MM.yyyy HH:mm}  |    {save.Header.Size,-2}    |  {aiMind}  ");
			}

			System.Console.WriteLine();

			System.Console.WriteLine();
			System.Console.WriteLine(_game.StateMsg.PadLeft(15));
			System.Console.WriteLine();
		}

		#endregion

		#region Input handlers

		private void Load(string input)
		{
			var number = int.Parse(input) - 1;

			if(number >= _saves.Count)
			{
				_game.StateMsg = Console.Game.MSG_BAD_INPUT;
				return;
			}

			var save = _saves[number];

			var data = LevelSaver.Load(save);

			if(data is null)
			{
				_game.StateMsg = MSG_SAVE_CORRUPTED;

				return;
			}

			var engine = new Console.Engine(data);

			_game.SetState(new PlayingState(_game, engine));
		}

		private void Remove(string input)
		{
			var number = int.Parse(input.Substring(3).Trim()) - 1;

			if (number >= _saves.Count)
			{
				_game.StateMsg = Console.Game.MSG_BAD_INPUT;
				return;
			}

			var save = _saves[number];

			if (LevelSaver.Remove(save))
			{
				_game.StateMsg = MSG_SAVE_REMOVE_SUCCESS;

				_saves.Remove(save);
			}
			else
			{
				_game.StateMsg = MSG_SAVE_REMOVE_FAIL;
			}
		}

		#endregion

		#region Initialization

		private void InitInputHandler()
		{
			_inputHandler.Add(REGEX_EXIT, (_) => { _game.SetState(new MainMenuState(_game)); });
			_inputHandler.Add(REGEX_LOAD, Load);
			_inputHandler.Add(REGEX_REMOVE, Remove);
		}

		#endregion
	}
}

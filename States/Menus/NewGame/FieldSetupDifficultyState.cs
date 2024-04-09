using SeaBattles.Console.Input;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
	internal class FieldSetupDifficultyState : IState
	{
		private const string REGEX_EASY_AI = "^1$";
		private const string REGEX_NORMAL_AI = "^2$";
		private const string REGEX_HARD_AI = "^3$";
		private const string REGEX_EXIT = @"^konc$";

		private readonly Game _game;
		private readonly FieldSetup _setup;

		private readonly InputHandler _inputHandler;

		public FieldSetupDifficultyState(Game game, FieldSetup setup)
		{
			_game = game;
			_setup = setup;

			_inputHandler = new();

			InitInputHandler();
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

		#region Input handlers

		private void SetEasyAI()
		{
			_setup.Difficult = Difficult.Easy;

			_game.SetState(new FieldCreatingState(_game, _setup));
		}

		private void SetNormalAI()
		{
			_setup.Difficult = Difficult.Normal;

			_game.SetState(new FieldCreatingState(_game, _setup));
		}

		private void SetHardAI()
		{
			_setup.Difficult = Difficult.Hard;

			_game.SetState(new FieldCreatingState(_game, _setup));
		}

		private void Exit()
		{
			_game.SetState(new MainMenuState(_game));
		}

		#endregion

		#region Drawing

		private void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine("Zvolte si rozum Umele Inteligence:");
			System.Console.WriteLine("1. Hloupy");
			System.Console.WriteLine("2. Chytry");
			System.Console.WriteLine("3. Moudry");

			System.Console.WriteLine();
			System.Console.WriteLine("Pokud se chcete vratit do hladniho menu, zadejte \'konc\'.");
			System.Console.WriteLine();

			System.Console.WriteLine(_game.StateMsg.PadLeft(15));
			System.Console.WriteLine();
		}

		#endregion

		#region Initialization

		private void InitInputHandler()
		{
			_inputHandler.Add(REGEX_EASY_AI, (_) => { SetEasyAI(); });
			_inputHandler.Add(REGEX_NORMAL_AI, (_) => { SetNormalAI(); });
			_inputHandler.Add(REGEX_HARD_AI, (_) => { SetHardAI(); });
			_inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
		}

		#endregion
	}
}

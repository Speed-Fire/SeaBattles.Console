using SeaBattles.Console.Input;

namespace SeaBattles.Console.States.Menus
{
	internal class MainMenuState : IState
	{
		private readonly Game _game;

		private readonly InputHandler _inputHandler = new();

		public MainMenuState(Game game)
		{
			_game = game;

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

		#region Drawing

		private void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine("Vitam Vas ve hre Lode!");
			System.Console.WriteLine("Zvolte prosim cislo pro pokracovani:");
			System.Console.WriteLine("1. Nova hra");
			System.Console.WriteLine("2. Nahrat hru");
			System.Console.WriteLine("3. Staci");

			System.Console.WriteLine();
			System.Console.WriteLine(_game.StateMsg.PadLeft(15));
			System.Console.WriteLine();
		}

		#endregion

		#region Initialization

		private void InitInputHandler()
		{
			_inputHandler.Add(@"^1$", (_) => { _game.SetState(new FieldSetupSizeState(_game)); });
			_inputHandler.Add(@"^2$", (_) => { _game.SetState(new LoadGameState(_game)); });
			_inputHandler.Add(@"^3$", (_) => { _game.SetState(null); });
		}

		#endregion
	}
}

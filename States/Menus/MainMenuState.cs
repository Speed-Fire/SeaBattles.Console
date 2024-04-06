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

			_inputHandler.Handle(input);
		}

		#region Drawing

		private static void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine("Vitam Vas ve hre Lode!");
			System.Console.WriteLine("Zvolte prosim cislo pro pokracovani:");
			System.Console.WriteLine("1. Nova hra");
			System.Console.WriteLine("2. Nahrat hru");
			System.Console.WriteLine("3. Staci");
		}

		#endregion

		#region Initialization

		private void InitInputHandler()
		{
			_inputHandler.Add(@"^1$", (_) => { _game.SetState(new FieldSetupState(_game)); });
			_inputHandler.Add(@"^2$", (_) => { _game.SetState(new LoadGameState(_game)); });
			_inputHandler.Add(@"^3$", (_) => { _game.SetState(null); });
		}

		#endregion
	}
}

using SeaBattles.Console.Input;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
	internal class FieldSetupDifficultyState : UserInputState<Game>
	{
		private const string REGEX_AI = @"^\d*$";
		private const string REGEX_EXIT = @"^konc$";

		private readonly FieldSetup _setup;

		public FieldSetupDifficultyState(Game game, FieldSetup setup)
			: base(game)
		{
			_setup = setup;
		}

		#region Input handlers

		private void SetAI(string input)
		{
			var val = int.Parse(input);

			if(val < 1 || val > 3)
			{
				StateMsg = MSG_BAD_INPUT;

				return;
			}

			_setup.Difficulty = val switch
			{
				1 => Difficulty.Easy,
				2 => Difficulty.Normal,
				3 => Difficulty.Hard,
				_ => Difficulty.Hard
			};

			SetState(new FieldCreatingState(StateMachine, _setup));
		}

		private void Exit()
		{
			SetState(new MainMenuState(StateMachine));
		}

		#endregion

		#region Drawing

		protected override void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine("Zvolte si rozum Umele Inteligence:");

			var color = System.Console.ForegroundColor;

			System.Console.Write("1. ");

			System.Console.ForegroundColor = ConsoleColor.DarkGreen;
			System.Console.WriteLine("Hloupy");
			System.Console.ForegroundColor = color;

			System.Console.Write("2. ");

			System.Console.ForegroundColor = ConsoleColor.DarkYellow;
			System.Console.WriteLine("Chytry");
			System.Console.ForegroundColor = color;

			System.Console.Write("3. ");

			System.Console.ForegroundColor = ConsoleColor.DarkRed;
			System.Console.WriteLine("Moudry");
			System.Console.ForegroundColor = color;


			System.Console.WriteLine();
			System.Console.WriteLine("Pokud se chcete vratit do hlavniho menu, zadejte \'konc\'.");
		}

		#endregion

		#region Initialization

		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add(REGEX_AI, SetAI);
			inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
		}

		#endregion
	}
}

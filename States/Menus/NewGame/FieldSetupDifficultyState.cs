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

			_setup.Difficult = val switch
			{
				1 => Difficult.Easy,
				2 => Difficult.Normal,
				3 => Difficult.Hard,
				_ => Difficult.Hard
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
			System.Console.WriteLine("1. Hloupy");
			System.Console.WriteLine("2. Chytry");
			System.Console.WriteLine("3. Moudry");

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

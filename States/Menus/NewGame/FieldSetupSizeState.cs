using SeaBattles.Console.Input;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
	internal class FieldSetupSizeState : UserInputState<Game>
	{
		private const string REGEX_SIZE = @"^\d+$";
		private const string REGEX_EXIT = @"^konc$";

		private const int MIN_FIELD_SIZE = 7;
		private const int MAX_FIELD_SIZE = 15;

		public FieldSetupSizeState(Game game) : base(game)
		{

		}

		#region Drawing

		protected override void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine("Zadejte velikost plochy:");
			System.Console.WriteLine($"   min: {MIN_FIELD_SIZE};   max: {MAX_FIELD_SIZE}");

			System.Console.WriteLine();
			System.Console.WriteLine("Pokud se chcete vratit do hlavniho menu, zadejte \'konc\'.");
		}

		#endregion

		#region Input handlers

		private void GetSize(string input)
		{
			if (int.TryParse(input, out var size)
					&& size >= MIN_FIELD_SIZE
					&& size <= MAX_FIELD_SIZE)
			{
				SetState(new FieldSetupDifficultyState(StateMachine, new FieldSetup() { Size = size }));
			}
			else
			{
				StateMsg = MSG_BAD_INPUT;
			}
		}

		private void Exit()
		{
			SetState(new MainMenuState(StateMachine));
		}

		#endregion

		#region Initialization

		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add(REGEX_SIZE, GetSize);
			inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
		}

		#endregion
	}
}

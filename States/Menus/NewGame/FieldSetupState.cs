using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
	internal class FieldSetupState : IState
	{
		private readonly Game _game;

		private const int MIN_FIELD_SIZE = 7;
		private const int MAX_FIELD_SIZE = 15;

		public FieldSetupState(Game game)
		{
			_game = game;
		}

		private static int GetSize()
		{
			do
			{
				Print();

				var str = System.Console.ReadLine();

				if (int.TryParse(str, out var size)
					&& size >= MIN_FIELD_SIZE
					&& size <= MAX_FIELD_SIZE)
				{
					return size;
				}
			} while (true);
		}

		private static void Print()
		{
			System.Console.Clear();

			System.Console.WriteLine("Zadejte velikost plochy:");
			System.Console.WriteLine($"   min: {MIN_FIELD_SIZE};   max: {MAX_FIELD_SIZE}");
		}

		public void Invoke()
		{
			var size = GetSize();

			_game.SetState(new FieldCreatingState(_game, new FieldSetup(size)));
		}
	}
}

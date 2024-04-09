using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	internal class PlayerMoveResultState : IState
	{
		private readonly Console.Engine _engine;

		public PlayerMoveResultState(Console.Engine engine)
		{
			_engine = engine;
		}

		public void Invoke()
		{
			Draw();

			System.Console.ReadLine();

			_engine.SetState(new AIMoveState(_engine));
		}

		#region Drawing

		private void Draw()
		{
			System.Console.Clear();

			var moveStr = "====    Vas tah    ====";

			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '='));
			System.Console.WriteLine(moveStr);
			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '='));

			System.Console.WriteLine();
			System.Console.WriteLine($"Napovedy: {_engine.LevelData.RemainingHintCount}. Pis \'nap\'");
			System.Console.WriteLine();

			System.Console.WriteLine("Zbyva lodi:");
			System.Console.WriteLine("   Vase          Pocitacove");
			System.Console.WriteLine($"    {_engine.LevelData.UserField.ShipCount,-2}               {_engine.LevelData.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Pocitacova plocha:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(_engine.LevelData.CompField, true, _engine.LevelData.Hints); // set false only to debug purpose

			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine(_engine.StateMsg.PadLeft(15));
			System.Console.WriteLine();

			System.Console.WriteLine("Pokracujte stiskem libovolne klavesy...");
		}

		#endregion
	}
}

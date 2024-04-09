using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	internal class DefeatState : IState
	{
		private readonly Console.Engine _engine;

		public DefeatState(Console.Engine engine)
		{
			_engine = engine;
		}

		public void Invoke()
		{
			Draw();

			System.Console.ReadLine();

			_engine.SetState(null);
		}

		#region Drawing

		private void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine();

			System.Console.WriteLine("=======================");
			System.Console.WriteLine("====    PORAZKA    ====");
			System.Console.WriteLine("=======================");

			System.Console.WriteLine();
			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine("Zbyva lodi:");
			System.Console.WriteLine("   Vase          Pocitacove");
			System.Console.WriteLine($"    {_engine.LevelData.UserField.ShipCount,-2}               {_engine.LevelData.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Vase plocha:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(_engine.LevelData.UserField, false);

			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine(_engine.StateMsg.PadLeft(15));
			System.Console.WriteLine();

			System.Console.WriteLine("Pokracujte stiskem libovolne klavesy...");
		}

		#endregion
	}
}

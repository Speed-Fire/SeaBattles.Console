﻿using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	internal class DefeatState : EngineState
	{
		public DefeatState(Console.Engine engine, Func<string, IState> stateGetter) 
			: base(engine, stateGetter)
		{
		}

		public override void Invoke()
		{
			Draw();

			System.Console.ReadLine();

			SetNullState();
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
			System.Console.WriteLine($"    {_engine.UserField.ShipCount,-2}               {_engine.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Vase plocha:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(_engine.UserField, false);

			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine(_engine.StateMsg.PadLeft(15));
			System.Console.WriteLine();

			System.Console.WriteLine("Pokracujte stiskem libovolne klavesy...");
		}

		#endregion
	}
}

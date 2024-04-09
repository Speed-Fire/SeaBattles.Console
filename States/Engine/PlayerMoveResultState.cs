﻿using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	internal class PlayerMoveResultState : EngineState
	{
		public PlayerMoveResultState(Console.Engine engine, Func<string, IState> stateGetter)
			: base(engine, stateGetter)
		{
		}

		public override void Invoke()
		{
			Draw();

			System.Console.ReadLine();

			SetState(nameof(AIMoveState));
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
			System.Console.WriteLine($"Napovedy: {_engine.RemainingHintCount}. Pis \'nap\'");
			System.Console.WriteLine();

			System.Console.WriteLine("Zbyva lodi:");
			System.Console.WriteLine("   Vase          Pocitacove");
			System.Console.WriteLine($"    {_engine.UserField.ShipCount,-2}               {_engine.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Pocitacova plocha:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(_engine.CompField, true, _engine.Hints); // set false only to debug purpose

			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine(_engine.StateMsg.PadLeft(15));
			System.Console.WriteLine();

			System.Console.WriteLine("Pokracujte stiskem libovolne klavesy...");
		}

		#endregion
	}
}

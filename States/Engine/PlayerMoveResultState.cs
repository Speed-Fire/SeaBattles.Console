﻿using SeaBattles.Console.Input;
using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	internal class PlayerMoveResultState : UserInputState<Console.Engine>
	{
		private readonly string _moveMsg;

		public PlayerMoveResultState(Console.Engine engine, string stateMsg)
			: base(engine)
		{
			_moveMsg = stateMsg;
		}

		#region Drawing

		protected override void Draw()
		{
			System.Console.Clear();

			var moveStr = "====    Vas tah    ====";

			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '='));
			System.Console.WriteLine(moveStr);
			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '='));

			System.Console.WriteLine();
			System.Console.WriteLine($"Napovedy: {StateMachine.LevelData.RemainingHintCount}.");
			System.Console.WriteLine();

			System.Console.WriteLine("Zbyva lodi:");
			System.Console.WriteLine("   Vase          Pocitacove");
			System.Console.WriteLine($"    {StateMachine.LevelData.UserField.ShipCount,-2}               {StateMachine.LevelData.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Pocitacova plocha:");
			System.Console.WriteLine();

			// set false only to debug purpose
			BattlefieldDrawer.Draw(StateMachine.LevelData.CompField, true, StateMachine.LevelData.Hints);

			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine(_moveMsg.PadLeft(15));
			System.Console.WriteLine();

			System.Console.WriteLine("Pokracujte stiskem libovolne klavesy...");
		}

		#endregion

		#region Initialization

		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add("^.*$", (_) => { SetState(new AIMoveState(StateMachine)); });
		}

		#endregion
	}
}

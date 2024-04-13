using SeaBattles.Console.Input;
using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	internal class VictoryState : UserInputState<Console.Engine>
	{
		private readonly string _moveMsg;

		public VictoryState(Console.Engine engine, string moveMsg)
			: base(engine)
		{
			_moveMsg = moveMsg;
		}

		#region Drawing

		protected override void Draw()
		{
			System.Console.Clear();

			var color = System.Console.ForegroundColor;

			System.Console.ForegroundColor = ConsoleColor.DarkGreen;

			System.Console.WriteLine("=======================");
			System.Console.WriteLine("====     VYHRA     ====");
			System.Console.WriteLine("=======================");

			System.Console.ForegroundColor = color;

			System.Console.WriteLine();
			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine("Zbyva lodi:");
			System.Console.WriteLine("   Vase          Pocitacove");
			System.Console.WriteLine($"    {StateMachine.LevelData.UserField.ShipCount,-2}               {StateMachine.LevelData.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Pocitacova plocha:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(StateMachine.LevelData.CompField, true, StateMachine.LevelData.Hints); // set false only to debug purpose

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
			inputHandler.Add("^.*$", (_) => { SetState(null); });
		}

		#endregion
	}
}

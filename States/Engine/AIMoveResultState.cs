using SeaBattles.Console.Input;
using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	/// <summary>
	/// Trida reprezentujici stav vysledku tahu pocitace.
	/// </summary>
	internal class AIMoveResultState : UserInputState<Console.Engine>
	{
		private readonly string _moveMsg;

		public AIMoveResultState(Console.Engine engine, string moveMsg)
			: base(engine)
		{
			_moveMsg = moveMsg;
		}

		#region Drawing

		/// <summary>
		/// Metoda pro vykresleni stavu vysledku tahu pocitace.
		/// </summary>
		protected override void Draw()
		{
			System.Console.Clear();

			var moveStr = $"==== Pocitacuv tah ====";

			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '='));
			System.Console.WriteLine(moveStr);
			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '='));

			System.Console.WriteLine();
			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine("Zbyva lodi:");
			System.Console.WriteLine("   Vase          Pocitacove");
			System.Console.WriteLine($"    {StateMachine.LevelData.UserField.ShipCount,-2}               {StateMachine.LevelData.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Vase plocha:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(StateMachine.LevelData.UserField, false);

			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine(_moveMsg.PadLeft(15));
			System.Console.WriteLine();

			System.Console.WriteLine("Pokracujte stiskem libovolne klavesy...");
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Metoda pro inicializaci zpracovani vstupu pro stav vysledku tahu pocitace.
		/// </summary>
		/// <param name="inputHandler">Instance tridy InputHandler pro registraci tokenu.</param>
		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add("^.*$", (_) => { SetState(new PlayerMoveState(StateMachine)); });
		}

		#endregion
	}
}

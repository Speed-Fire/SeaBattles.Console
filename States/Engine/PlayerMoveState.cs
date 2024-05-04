using SeaBattles.Console.Input;
using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	/// <summary>
	/// Trida reprezentujici stav tahu uzivatele.
	/// </summary>
	internal class PlayerMoveState : UserInputState<Console.Engine>
	{
		private const string REGEX_SAVE_AND_EXIT = "^uloz$";
		private const string REGEX_USE_HINT = "^nap$";
		private const string REGEX_MOVE = @"^[a-zA-Z]\s*\d{1,2}$";
		private const string REGEX_EXIT = @"^konc$";

		private string _moveMsg = string.Empty;

		public PlayerMoveState(Console.Engine engine) : base(engine)
		{

		}

		#region Drawing

		/// <summary>
		/// Metoda pro vykresleni stavu tahu uzivatele.
		/// </summary>
		protected override void Draw()
		{
			System.Console.Clear();

			var moveStr = "====    Vas tah    ====";

			var saveTipStr = $"   Pokud chcete ulozit a ukoncit hru, zadejte \'uloz\'.";
			var exitTipStr = "   Pokud chcete ukoncit hru, zadejte \'konc\'.";

			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '='));
			System.Console.WriteLine(moveStr + saveTipStr);
			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '=') + exitTipStr);

			System.Console.WriteLine();
			System.Console.WriteLine($"Napovedy: {StateMachine.LevelData.RemainingHintCount}. {(StateMachine.LevelData.RemainingHintCount > 0 ? "Pis \'nap\'" : string.Empty)}");
			System.Console.WriteLine();

			System.Console.WriteLine("Zbyva lodi:");
			System.Console.WriteLine("   Vase          Pocitacove");
			System.Console.WriteLine($"    {StateMachine.LevelData.UserField.ShipCount,-2}               {StateMachine.LevelData.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Pocitacova plocha:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(StateMachine.LevelData.CompField, true, StateMachine.LevelData.Hints); // set false only to debug purpose

			System.Console.WriteLine();

			System.Console.WriteLine(_moveMsg.PadLeft(15));
			System.Console.WriteLine();
		}

		#endregion

		#region Input handlers

		/// <summary>
		/// Pokusi utocit nejakou bunku.
		/// </summary>
		/// <param name="input"></param>
		private void TakeMove(string input)
		{
			// parsovani a kontrola vstupu
			if (!ShipCoordinateParser
				.TryParse(input, StateMachine.LevelData.CompField.Size, out var x, out var y))
			{
				StateMsg = MSG_BAD_INPUT;

				return;
			}

			// kontrola stavu bunky, na kterou se pokusil utocit uzivatel:
			//   pokud je uz utocena anebo znicena, pak hlasi spatny vstup.
			if (StateMachine.LevelData.CompField[x, y] == CellState.Attacked ||
				StateMachine.LevelData.CompField[x, y] == CellState.Destroyed)
			{
				StateMsg = MSG_BAD_INPUT;

				return;
			}

			// utok
			var res = StateMachine.LevelData.CompField.Attack((uint)x, (uint)y);

			// kdyz po uteku nezbyva zadna lod na pocitacovem poli, pak
			//  prevede automat na stav vyhry.
			if (StateMachine.LevelData.CompField.IsEmpty)
			{
				SetState(new VictoryState(StateMachine, Console.Engine.MSG_SHIP_DESTROYED));

				return;
			}

			switch (res)
			{
				// neco se stalo spatne pri utoku.
				case AttackResult.Failed:
				default:
					StateMsg = MSG_BAD_INPUT;

					return;

				// v bunce nebylo lode.
				//  preved automat na stav vysledku tahu uzivatele.
				case AttackResult.Missed:
					StateMachine
						.SetState(new PlayerMoveResultState(StateMachine, Console.Engine.MSG_FIRE_MISSED));

					return;

				// lod byla porazena po utoku.
				case AttackResult.Hitten:
					_moveMsg = Console.Engine.MSG_SHIP_HIT;

					return;

				// lod byla znicena po utoku.
				case AttackResult.Destroyed:
					_moveMsg = Console.Engine.MSG_SHIP_DESTROYED;

					return;
			}
		}

		/// <summary>
		/// Prevede automat na stav ulozeni hry.
		/// </summary>
		private void SaveAndExit()
		{
			SetState(new SavingState(StateMachine));
		}

		/// <summary>
		/// Pouzije napoved
		/// </summary>
		private void UseHint()
		{
			if (StateMachine.LevelData.RemainingHintCount <= 0)
				return;

			while (true)
			{
				var hint = StateMachine.LevelData.CompField.GetRandomShipCell();

				if (hint is null || StateMachine.LevelData.AddHint(hint.Value))
					break;
			}
		}

		/// <summary>
		/// Vrati do hlavniho menu.
		/// </summary>
		private void Exit()
		{
			SetState(null);
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Metoda pro inicializaci zpracovani vstupu pro stav tahu uzivatele.
		/// </summary>
		/// <param name="inputHandler">Instance tridy InputHandler pro registraci tokenu.</param>
		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add(REGEX_MOVE, TakeMove);
			inputHandler.Add(REGEX_USE_HINT, (_) => { UseHint(); });
			inputHandler.Add(REGEX_SAVE_AND_EXIT, (_) => { SaveAndExit(); });
			inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
		}

		#endregion
	}
}

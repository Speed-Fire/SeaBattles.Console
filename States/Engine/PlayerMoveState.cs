using SeaBattles.Console.Input;
using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
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
			System.Console.WriteLine($"Napovedy: {StateMachine.LevelData.RemainingHintCount}. Pis \'nap\'");
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

		private void TakeMove(string input)
		{
			if (!ShipCoordinateParser.TryParse(input, StateMachine.LevelData.CompField.Size, out var x, out var y))
			{
				StateMsg = MSG_BAD_INPUT;

				return;
			}

			if (StateMachine.LevelData.CompField[x, y] == CellState.Attacked ||
				StateMachine.LevelData.CompField[x, y] == CellState.Destroyed)
			{
				StateMsg = MSG_BAD_INPUT;

				return;
			}

			var res = StateMachine.LevelData.CompField.Attack((uint)x, (uint)y);

			if (StateMachine.LevelData.CompField.IsEmpty)
			{
				SetState(new VictoryState(StateMachine, Console.Engine.MSG_SHIP_DESTROYED));

				return;
			}

			switch (res)
			{
				case AttackResult.Failed:
				default:
					StateMsg = MSG_BAD_INPUT;

					return;
				case AttackResult.Missed:
					StateMachine.SetState(new PlayerMoveResultState(StateMachine, Console.Engine.MSG_FIRE_MISSED));

					return;
				case AttackResult.Hitten:
					_moveMsg = Console.Engine.MSG_SHIP_HIT;

					return;
				case AttackResult.Destroyed:
					_moveMsg = Console.Engine.MSG_SHIP_DESTROYED;

					return;
			}
		}

		private void SaveAndExit()
		{
			SetState(new SavingState(StateMachine));
		}

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

		private void Exit()
		{
			SetState(null);
		}

		#endregion

		#region Initialization

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

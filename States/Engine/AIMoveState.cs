using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	internal class AIMoveState : StateBase<Console.Engine>
	{
		private const int AI_MIN_THINKING_DURATION = 1000;
		private const int AI_MAX_THINKING_DURATION = 5000;

		private string _moveMsg;

		public AIMoveState(Console.Engine engine)
			: base(engine)
		{
			_moveMsg = string.Empty;
		}

		public override void Invoke()
		{
			Draw();

			PretendComputerNeedMoreTimeForThinking();

			TakeMove();
		}

		#region Drawing

		private void Draw()
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
		}

		#endregion

		#region AI

		private static void PretendComputerNeedMoreTimeForThinking()
		{
			var random = new Random();

			var duration = random.Next(AI_MIN_THINKING_DURATION, AI_MAX_THINKING_DURATION);

			Thread.Sleep(duration);
		}

		private void TakeMove()
		{
			while (true)
			{
				var attackRes = StateMachine.LevelData.AI.Attack();

				if (StateMachine.LevelData.UserField.IsEmpty)
				{
					SetState(new DefeatState(StateMachine, Console.Engine.MSG_SHIP_DESTROYED));

					return;
				}

				switch (attackRes)
				{
					case AttackResult.Failed:
					default:
						return;
					case AttackResult.Missed:
						SetState(new AIMoveResultState(StateMachine, Console.Engine.MSG_FIRE_MISSED));

						return;
					case AttackResult.Hitten:
						_moveMsg = Console.Engine.MSG_SHIP_HIT;

						return;
					case AttackResult.Destroyed:
						_moveMsg = Console.Engine.MSG_SHIP_DESTROYED;

						return;
				}
			}
		}

		#endregion
	}
}

using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Game
{
	internal class AIMoveState : IState
	{
		private const int AI_MIN_THINKING_DURATION = 1000;
		private const int AI_MAX_THINKING_DURATION = 5000;

		private readonly Engine _engine;

		public AIMoveState(Engine engine)
		{
			_engine = engine;
		}

		public void Invoke()
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
			System.Console.WriteLine($"    {_engine.UserField.ShipCount,-2}               {_engine.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Vase plocha:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(_engine.UserField, false);

			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine(_engine.StateMsg.PadLeft(15));
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
				int x, y;

				while (true)
				{
					var res = ComputerFieldCoordsGenerator.Generate(_engine.UserField.Size);
					(x, y) = (res.X, res.Y);

					if (_engine.UserField[x, y] == CellState.Attacked ||
						_engine.UserField[x, y] == CellState.Destroyed)
						continue;

					break;
				}

				var attackRes = _engine.UserField.Attack((uint)x, (uint)y);

				if (_engine.UserField.IsEmpty)
				{
					_engine.SetState(new DefeatState(_engine));

					return;
				}

				switch (attackRes)
				{
					case AttackResult.Failed:
					default:
						_engine.StateMsg = Engine.MSG_BAD_INPUT;

						//_engine.SetState(new AIMoveState(_engine));

						return;
					case AttackResult.Missed:
						_engine.StateMsg = Engine.MSG_FIRE_MISSED;

						_engine.SetState(new AIMoveResultState(_engine));

						return;
					case AttackResult.Hitten:
						_engine.StateMsg = Engine.MSG_SHIP_HIT;

						//_engine.SetState(new AIMoveState(_engine));

						return;
					case AttackResult.Destroyed:
						_engine.StateMsg = Engine.MSG_SHIP_DESTROYED;

						//_engine.SetState(new AIMoveState(_engine));
						
						return;
				}
			}
		}

		#endregion
	}
}

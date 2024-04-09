using SeaBattles.Console.AI;
using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	internal class AIMoveState : IState
	{
		private const int AI_MIN_THINKING_DURATION = 1000;
		private const int AI_MAX_THINKING_DURATION = 5000;

		private readonly Console.Engine _engine;

		public AIMoveState(Console.Engine engine)
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
			System.Console.WriteLine($"    {_engine.LevelData.UserField.ShipCount,-2}               {_engine.LevelData.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Vase plocha:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(_engine.LevelData.UserField, false);

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
				var attackRes = _engine.LevelData.AI.Attack();

				if (_engine.LevelData.UserField.IsEmpty)
				{
					_engine.SetState(new DefeatState(_engine));

					return;
				}

				switch (attackRes)
				{
					case AttackResult.Failed:
					default:
						_engine.StateMsg = Console.Engine.MSG_BAD_INPUT;

						//_engine.SetState(new AIMoveState(_engine));

						return;
					case AttackResult.Missed:
						_engine.StateMsg = Console.Engine.MSG_FIRE_MISSED;

						_engine.SetState(new AIMoveResultState(_engine));

						return;
					case AttackResult.Hitten:
						_engine.StateMsg = Console.Engine.MSG_SHIP_HIT;

						//_engine.SetState(new AIMoveState(_engine));

						return;
					case AttackResult.Destroyed:
						_engine.StateMsg = Console.Engine.MSG_SHIP_DESTROYED;

						//_engine.SetState(new AIMoveState(_engine));
						
						return;
				}
			}
		}

		#endregion
	}
}

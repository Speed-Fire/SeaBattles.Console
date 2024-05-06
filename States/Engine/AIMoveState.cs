using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	/// <summary>
	/// Trida reprezentujici stav tahu pocitace.
	/// </summary>
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

		/// <summary>
		/// Metoda pro spusteni tohohle stavu.
		/// </summary>
		public override void Invoke()
		{
			Draw();

			PretendComputerNeedMoreTimeForThinking();

			TakeMove();
		}

		#region Drawing

		/// <summary>
		/// Metoda pro vykresleni stavu tahu pocitace.
		/// </summary>
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
			System.Console.WriteLine($"  Vase pole:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(StateMachine.LevelData.UserField, false);

			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine(_moveMsg.PadLeft(15));
			System.Console.WriteLine();
		}

		#endregion

		#region AI

		/// <summary>
		/// Predstira, ze pocitac potrebuje cas na promysleni sveho tahu.
		/// </summary>
		private static void PretendComputerNeedMoreTimeForThinking()
		{
			var random = new Random();

			var duration = random.Next(AI_MIN_THINKING_DURATION, AI_MAX_THINKING_DURATION);

			Thread.Sleep(duration);
		}

		/// <summary>
		/// Utoci nejakou bunku.
		/// </summary>
		private void TakeMove()
		{
			while (true)
			{
				// provadeni pocitacem utoku.
				var attackRes = StateMachine.LevelData.AI.Attack();

				// pokud po utoky byly zniceny vsechny lode uzivatele,
				//  pak prevede automat na stav prohry.
				if (StateMachine.LevelData.UserField.IsEmpty)
				{
					SetState(new DefeatState(StateMachine, Console.Engine.MSG_SHIP_DESTROYED));

					return;
				}

				switch (attackRes)
				{
					// neco se stalo spatne.
					case AttackResult.Failed:
					default:
						return;

					// prevede automat na stav vysledku pocitacoveho tahu.
					case AttackResult.Missed:
						SetState(new AIMoveResultState(StateMachine, Console.Engine.MSG_FIRE_MISSED));

						return;

					// nahlasi porazeni lode.
					case AttackResult.Hitten:
						_moveMsg = Console.Engine.MSG_SHIP_HIT;

						return;

					// nahlasi zniceni lode.
					case AttackResult.Destroyed:
						_moveMsg = Console.Engine.MSG_SHIP_DESTROYED;

						return;
				}
			}
		}

		#endregion
	}
}

using SeaBattles.Console.States.Engine;

namespace SeaBattles.Console
{
	internal class Engine : StateMachine
	{
		public const string MSG_SHIP_HIT = "porazeni";
		public const string MSG_SHIP_DESTROYED = "znicena";
		public const string MSG_FIRE_MISSED = "minuti cile";

		public BattleField UserField { get; }
		public BattleField CompField { get; }

		public List<(int, int)> Hints { get; } = new();

		public int RemainingHintCount { get; private set; } = 3;

		public Engine(BattleField userField, BattleField compField)
		{
			UserField = userField;
			CompField = compField;

			SetState(new PlayerMoveState(this));
		}

		public void AddHint((int, int) hint)
		{
			if (RemainingHintCount <= 0)
				return;

			Hints.Add(hint);

			RemainingHintCount--;
		}
	}
}

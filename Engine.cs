using SeaBattles.Console.States;
using SeaBattles.Console.States.Game;

namespace SeaBattles.Console
{
	internal class Engine
	{
		public const string MSG_SHIP_HIT = "porazeni";
		public const string MSG_SHIP_DESTROYED = "znicena";
		public const string MSG_FIRE_MISSED = "minuti cile";
		public const string MSG_BAD_INPUT = "spatny vstup";

		private IState? CurrentState { get; set; }

		public BattleField UserField { get; }
		public BattleField CompField { get; }

		public List<(int, int)> Hints { get; } = new();

		public int RemainingHintCount { get; private set; } = 3;

		private string _stateMsg = string.Empty;
		public string StateMsg
		{
			get
			{
				var tmp = _stateMsg;

				_stateMsg = string.Empty;

				return tmp;
			}

			set
			{
				_stateMsg = value;
			}
		}

		public Engine(BattleField userField, BattleField compField)
		{
			UserField = userField;
			CompField = compField;

			SetState(new PlayerMoveState(this));
		}

		public void Start()
		{
			while(CurrentState is not null)
			{
				CurrentState.Invoke();
			}
		}

		public void AddHint((int, int) hint)
		{
			if (RemainingHintCount <= 0)
				return;

			Hints.Add(hint);

			RemainingHintCount--;
		}

		#region State machine

		public void SetState(IState? state)
		{
			CurrentState = state;
		}

		#endregion
	}
}

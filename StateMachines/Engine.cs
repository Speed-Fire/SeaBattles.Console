using SeaBattles.Console.AI;
using SeaBattles.Console.States;
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

		private readonly Dictionary<string, IState> _stateMap;

		public Engine(BattleField userField, BattleField compField, AIPlayer ai)
		{
			UserField = userField;
			CompField = compField;

			_stateMap = new();

			InitStateMap(ai);

			SetState(_stateMap[nameof(PlayerMoveState)]);
		}

		public void AddHint((int, int) hint)
		{
			if (RemainingHintCount <= 0)
				return;

			Hints.Add(hint);

			RemainingHintCount--;
		}

		private void InitStateMap(AIPlayer ai)
		{
			IState StateGetter(string name)
			{
				return _stateMap[name];
			}

			_stateMap.Add(nameof(PlayerMoveState), new PlayerMoveState(this, StateGetter));
			_stateMap.Add(nameof(PlayerMoveResultState), new PlayerMoveResultState(this, StateGetter));
			_stateMap.Add(nameof(AIMoveState), new AIMoveState(this, StateGetter, ai));
			_stateMap.Add(nameof(AIMoveResultState), new AIMoveResultState(this, StateGetter));
			_stateMap.Add(nameof(VictoryState), new VictoryState(this, StateGetter));
			_stateMap.Add(nameof(DefeatState), new DefeatState(this, StateGetter));
		}
	}
}

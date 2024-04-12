using SeaBattles.Console.AI;

namespace SeaBattles.Console.Models
{
	internal class LevelData
	{
		public AIPlayer AI { get; }
		public BattleField UserField { get; }

		public BattleField CompField { get; }

		private readonly List<(int, int)> _hints;
		public IReadOnlyList<(int, int)> Hints => _hints;

		public int RemainingHintCount { get; private set; }


		public LevelData(AIPlayer aI,
			BattleField userField, BattleField compField,
			List<(int, int)> hints, int remainingHintCount)
		{
			AI = aI;
			UserField = userField;
			CompField = compField;
			_hints = hints;
			RemainingHintCount = remainingHintCount;
		}

		public LevelData(AIPlayer aI, BattleField userField, BattleField compField)
		{
			AI = aI;
			UserField = userField;
			CompField = compField;

			_hints = new();
			RemainingHintCount = 3;
		}

		public bool AddHint((int, int) hint)
		{
			if(RemainingHintCount > 0)
			{
				_hints.Add(hint);
				RemainingHintCount--;

				return true;
			}
			else
			{
				return false;
			}
		}
    }
}

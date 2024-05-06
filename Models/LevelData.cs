using SeaBattles.Console.AI;

namespace SeaBattles.Console.Models
{
	/// <summary>
	/// Trida reprezentujici data urovne.
	/// </summary>
	internal class LevelData
	{
		/// <summary>
		/// AI hrac.
		/// </summary>
		public AIPlayer AI { get; }

		/// <summary>
		/// Hraci pole uzivatele.
		/// </summary>
		public BattleField UserField { get; }

		/// <summary>
		/// Hraci pole pocitace.
		/// </summary>
		public BattleField CompField { get; }

		private readonly List<(int, int)> _hints;

		/// <summary>
		/// Seznam napoved pro hrace.
		/// </summary>
		public IReadOnlyList<(int, int)> Hints => _hints;

		/// <summary>
		/// Zbyvajici pocet napoved.
		/// </summary>
		public int RemainingHintCount { get; private set; }

		/// <summary>
		/// Konstruktor pro inicializaci dat urovne.
		/// </summary>
		/// <param name="aI">AI hrac.</param>
		/// <param name="userField">Hraci pole uzivatele.</param>
		/// <param name="compField">Hraci pole pocitace.</param>
		/// <param name="hints">Seznam napoved pro hrace.</param>
		/// <param name="remainingHintCount">Zbyvajici pocet napoved.</param>
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

		/// <summary>
		/// Konstruktor pro inicializaci dat urovne.
		/// </summary>
		/// <param name="aI">AI hrac.</param>
		/// <param name="userField">Hraci pole uzivatele.</param>
		/// <param name="compField">Hraci pole pocitace.</param>
		/// <param name="remainingHintCount">Zbyvajici pocet napovedi.</param>
		public LevelData(AIPlayer aI, BattleField userField, BattleField compField,
			int remainingHintCount)
		{
			AI = aI;
			UserField = userField;
			CompField = compField;

			_hints = new();
			RemainingHintCount = remainingHintCount;
		}

		/// <summary>
		/// Metoda pro pridani napovedy.
		/// </summary>
		/// <param name="hint">Napoved k pridani.</param>
		/// <returns>True, pokud byla napoved uspesne pridana; jinak false.</returns>
		public bool AddHint((int, int) hint)
		{
			// kdyz vsechny napovedi jsou vyuzite, vratu True.
			if (RemainingHintCount <= 0)
				return true;

			// pokud pouzite napovedi neobsahuji novou, pak ji prida
			//  a vrati True.
			if (!_hints.Contains(hint))
			{
				_hints.Add(hint);
				RemainingHintCount--;

				return true;
			}
			// jinak vrati False.
			else
			{
				return false;
			}
		}
	}

}

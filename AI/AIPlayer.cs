using SeaBattles.Console.Misc;

namespace SeaBattles.Console.AI
{
	/// <summary>
	/// Abstraktni trida reprezentujici umelou inteligenci hrace.
	/// </summary>
	internal abstract class AIPlayer
	{
		protected readonly BattleField _field;

		/// <summary>
		/// Inicializuje novou instanci tridy AIPlayer s danym bojovym polem.
		/// </summary>
		/// <param name="field">Bojove pole, na kterem bude hrat umela inteligence.</param>
		public AIPlayer(BattleField field)
		{
			_field = field;
		}

		/// <summary>
		/// Provadi utok a vraci vysledek.
		/// </summary>
		/// <returns>Vysledek utoku.</returns>
		public abstract AttackResult Attack();

		/// <summary>
		/// Generuje nahodne souradnice na bojovem poli.
		/// </summary>
		/// <returns>Krabicku s nahodnymi souradnicemi.</returns>
		protected (int, int) GetRandomCoords()
		{
			int x, y;

			while (true)
			{
				var res = ComputerFieldCoordsGenerator.Generate(_field.Size);
				(x, y) = (res.X, res.Y);

				if (_field[x, y] == CellState.Attacked ||
					_field[x, y] == CellState.Destroyed)
					continue;

				return (x, y);
			}
		}

		/// <summary>
		/// Prepisuje metodu ToString pro vraceni prazdneho retezce.
		/// </summary>
		/// <returns>Prazdny retezec.</returns>
		public override string ToString()
		{
			return string.Empty;
		}
	}
}

using System.Runtime.Serialization;

namespace SeaBattles.Console.AI
{
	internal class AIPlayerEasy : AIPlayer
	{
		public AIPlayerEasy(BattleField field) : base(field)
		{
		}

		/// <summary>
		/// Provadi utok a vraci vysledek.
		/// </summary>
		/// <returns>Vysledek utoku.</returns>
		public override AttackResult Attack()
		{
			var (x, y) = GetRandomCoords();

			var res = _field.Attack((uint)x, (uint)y);

			return res;
		}
	}
}

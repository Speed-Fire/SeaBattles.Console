﻿namespace SeaBattles.Console.AI
{
	internal class AIPlayerEasy : AIPlayer
	{
		public AIPlayerEasy(BattleField field) : base(field)
		{
		}

		public override AttackResult Attack()
		{
			var (x, y) = GetRandomCoords();

			var res = _field.Attack((uint)x, (uint)y);

			return res;
		}
	}
}
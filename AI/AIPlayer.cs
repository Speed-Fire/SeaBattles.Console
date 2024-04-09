﻿using SeaBattles.Console.Misc;

namespace SeaBattles.Console.AI
{
	internal abstract class AIPlayer
	{
		protected readonly BattleField _field;

		public AIPlayer(BattleField field)
		{
			_field = field;
		}

		public abstract AttackResult Attack();

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
	}
}

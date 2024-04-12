namespace SeaBattles.Console.AI
{
	internal class AIPlayerHard : AIPlayerNormal
	{
		public AIPlayerHard(BattleField field) : base(field)
		{
		}

        public AIPlayerHard(BattleField field, LastLucklyAttack? lastLucklyAttack)
			: base(field, lastLucklyAttack)
        {
            
        }

        protected override bool IsItWorthToAttackThisPoint(int x, int y)
		{
			var dirs = new (int, int)[] { (-1, -1), (1, -1), (1, -1), 
				(1, 1), (1, 0), (-1, 0), (0, 1), (0, -1) };

			var size = _field.Size;

			foreach (var (dirX, dirY) in dirs)
			{
				var shiftX = x + dirX;
				var shiftY = y + dirY;

				if (shiftX < 0 || shiftY < 0 || shiftX >= size || shiftY >= size)
					continue;

				if (_lastAttack is not null &&
					_lastAttack.lastHittenPoint.Item1 == shiftX &&
					_lastAttack.lastHittenPoint.Item2 == shiftY)
					continue;

				if (_field[shiftX, shiftY] == CellState.Destroyed)
					return false;
			}

			return true;
		}
	}
}

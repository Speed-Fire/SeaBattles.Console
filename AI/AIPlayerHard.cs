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

		/// <summary>
		/// Urcuje, zda je vhodne utocit na tento bod.
		/// </summary>
		/// <param name="x">X-ova souradnice bodu.</param>
		/// <param name="y">Y-ova souradnice bodu.</param>
		/// <returns>True, pokud je vhodne utocit, jinak false.</returns>
		protected override bool IsItWorthToAttackThisPoint(int x, int y)
		{
			// overuje, jsou-li vedle zvoleneho bodu bunky s lodi,
			//  a to neni ta sama lod, kterou se snazi znicit.
			//  Ale jen za predpokladu, ze AI vi o tom, ze vedle je
			//  znicena lod.

			var dirs = new (int, int)[] { (-1, -1), (1, -1), (1, -1), 
				(1, 1), (1, 0), (-1, 0), (0, 1), (0, -1) };

			var size = _field.Size;

			foreach (var (dirX, dirY) in dirs)
			{
				var shiftX = x + dirX;
				var shiftY = y + dirY;

				if (shiftX < 0 || shiftY < 0 || shiftX >= size || shiftY >= size)
					continue;

				// pokud vedle je lod, kterou snazi se znicit,
				//  overuje nasledujici bunku.
				if (_lastAttack is not null &&
					_lastAttack.lastHittenPoint.Item1 == shiftX &&
					_lastAttack.lastHittenPoint.Item2 == shiftY)
					continue;

				// pokud vedle je zniceny kus lode, pak
				//  vrati False.
				if (_field[shiftX, shiftY] == CellState.Destroyed)
					return false;
			}

			return true;
		}
	}
}

namespace SeaBattles.Console.AI
{
	internal class AIPlayerNormal : AIPlayer
	{
		public class LastLucklyAttack
		{
			public (int, int) lastHittenPoint;

			public int directionId;

			public int length;
		}

		protected readonly (int, int)[] _directions;

		protected LastLucklyAttack? _lastAttack = null;
		public LastLucklyAttack? LastAttack => _lastAttack is null ? null :
			new()
			{
				lastHittenPoint = _lastAttack.lastHittenPoint,
				directionId = _lastAttack.directionId,
				length = _lastAttack.length
			};

		public AIPlayerNormal(BattleField field) : base(field)
		{
			_directions = new (int, int)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
		}

        public AIPlayerNormal(BattleField field, LastLucklyAttack? lastLucklyAttack)
			: this(field) 
        {
            _lastAttack = lastLucklyAttack;
        }

        public override AttackResult Attack()
		{
			var (x, y) = PredictNextShipCell();

			var res = _field.Attack((uint)x, (uint)y);

			switch (res)
			{
				case AttackResult.Hitten:
					HitCorrection(x, y);
					break;
				case AttackResult.Destroyed:
					ClearLastAttack();
					break;
				case AttackResult.Missed:
					if (_lastAttack is not null)
						MissCorection();
					break;
				default:
					break;
			}

			return res;
		}

		private (int, int) PredictNextShipCell()
		{
			while (true)
			{
				int x, y;

				if (_lastAttack is null)
				{
					(x, y) = GetRandomCoords();
				}
				else
				{

					var (_x, _y) = _lastAttack.lastHittenPoint;
					var dir = _directions[_lastAttack.directionId];

					(x, y) = (_x + dir.Item1, _y + dir.Item2);
				}

				if (IsItWorthToAttackThisPoint(x, y) &&
					x >= 0 && y >= 0 && x < _field.Size && y < _field.Size &&
					_field[x, y] != CellState.Attacked &&
					_field[x, y] != CellState.Destroyed)
					return (x, y);

				MissCorection();
			}
		}

		protected virtual bool IsItWorthToAttackThisPoint(int x, int y)
		{
			return true;
		}

		private void HitCorrection(int x, int y)
		{
			if(_lastAttack is null)
			{
				var dirId = GetNextDirectionId(x, y);

				_lastAttack = new()
				{
					lastHittenPoint = (x, y),
					directionId = dirId,
					length = 1
				};
			}
			else
			{
				_lastAttack.lastHittenPoint = (x, y);
				_lastAttack.length++;
			}
		}

		private void MissCorection()
		{
			if (_lastAttack is null)
				return;

			if(_lastAttack.length == 1)
			{
				var (x, y) = _lastAttack.lastHittenPoint;
				var dirId = _lastAttack.directionId;

				_lastAttack.directionId = GetNextDirectionId(x, y, dirId + 1);
			}
			else
			{
				var dirId = GetOppositeDirection(_lastAttack.directionId);

				_lastAttack.directionId = dirId;

				var shiftX = _lastAttack.lastHittenPoint.Item1;
				var shiftY = _lastAttack.lastHittenPoint.Item2;

				var dir = _directions[dirId];

				for(int i = 0; i < _lastAttack.length - 1; i++)
				{
					shiftX += dir.Item1;
					shiftY += dir.Item2;
				}

				_lastAttack.lastHittenPoint = (shiftX, shiftY);
			}
		}

		private void ClearLastAttack()
		{
			_lastAttack = null;
		}

		private int GetNextDirectionId(int x, int y, int startId = 0)
		{
			int dirId;
			for (dirId = startId; dirId < _directions.Length; dirId++)
			{
				var dir = _directions[dirId];

				var shiftX = x + dir.Item1;
				var shiftY = y + dir.Item2;

				if (shiftX < 0 || shiftX >= _field.Size ||
				   shiftY < 0 || shiftY >= _field.Size)
					continue;

				return dirId;
			}

			return 0;
		}

		private static int GetOppositeDirection(int dirId)
		{
			return dirId switch
			{
				0 => 2,
				1 => 3,
				2 => 0,
				3 => 1,
				_ => 0,
			};
		}
	}
}

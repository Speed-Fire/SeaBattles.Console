namespace SeaBattles.Console.AI
{
	/// <summary>
	/// Trida AIPlayerNormal reprezentuje umeleho hrace s normalnim chovanim.
	/// </summary>
	internal class AIPlayerNormal : AIPlayer
	{
		/// <summary>
		/// Trida LastLucklyAttack uchovava informace o poslednim uspesnem utoku.
		/// </summary>
		public class LastLucklyAttack
		{
			public (int, int) lastHittenPoint;

			public int directionId;

			public int length;
		}

		protected readonly (int, int)[] _directions;

		protected LastLucklyAttack? _lastAttack = null;

		/// <summary>
		/// Posledni uspesny utok.
		/// </summary>
		public LastLucklyAttack? LastAttack => _lastAttack is null ? null :
			new()
			{
				lastHittenPoint = _lastAttack.lastHittenPoint,
				directionId = _lastAttack.directionId,
				length = _lastAttack.length
			};

		/// <summary>
		/// Inicializuje novou instanci tridy AIPlayerNormal s danym bojovym polem.
		/// </summary>
		/// <param name="field">Bojove pole, na kterem bude hrat umela inteligence.</param>
		public AIPlayerNormal(BattleField field) : base(field)
		{
			_directions = new (int, int)[] { (-1, 0), (0, 1), (1, 0), (0, -1) };
		}

		/// <summary>
		/// Inicializuje novou instanci tridy AIPlayerNormal s danym bojovym polem a poslednim uspesnym utokem.
		/// </summary>
		/// <param name="field">Bojove pole, na kterem bude hrat umela inteligence.</param>
		/// <param name="lastLucklyAttack">Posledni uspesny utok.</param>
		public AIPlayerNormal(BattleField field, LastLucklyAttack? lastLucklyAttack)
			: this(field)
		{
			_lastAttack = lastLucklyAttack;
		}

		/// <summary>
		/// Provadi utok a vraci vysledek.
		/// </summary>
		/// <returns>Vysledek utoku.</returns>
		public override AttackResult Attack()
		{
			// Kdyz naposledy lod byla porazena, ale neznicena,
			//  odhadne nasledujici moznou bunku s teto lodi.
			//  Jinak nahodne vybere bunku.
			var (x, y) = PredictNextShipCell();

			var res = _field.Attack((uint)x, (uint)y);

			switch (res)
			{
				// pokud lod byla porazena, provede korekturu
				//  uspesneho odhadnuti.
				case AttackResult.Hitten:
					HitCorrection(x, y);
					break;

				// Pokud lod byla znicena, smaze vsechny daty
				//  o poslednim utoku, pokud jsou.
				case AttackResult.Destroyed:
					ClearLastAttack();
					break;

				// pokud utok byl mimo, a drive byla porazena,
				//  ale neznicena lod, provede korekturu
				//  spatneho odhadnuti.
				//  Jinak neudela nic.
				case AttackResult.Missed:
					if (_lastAttack is not null)
						MissCorection();
					break;

				default:
					break;
			}

			return res;
		}

		/// <summary>
		/// Predikuje nasledujici bunku lode.
		/// </summary>
		/// <returns>Souradnice nasledujici bunky.</returns>
		private (int, int) PredictNextShipCell()
		{
			while (true)
			{
				int x, y;

				// nemame daty o porazene lodi.
				if (_lastAttack is null)
				{
					(x, y) = GetRandomCoords();
				}
				// mame daty o porazene lodi.
				else
				{

					var (_x, _y) = _lastAttack.lastHittenPoint;
					var dir = _directions[_lastAttack.directionId];

					(x, y) = (_x + dir.Item1, _y + dir.Item2);
				}

				// kontrola zvolene bunky:
				//  - je-li mimo pole;
				//  - bunka obsahuje znicenou lod anebo je jiz utocena;
				//  - nedava smysl utocit tuto bunku.
				if (IsItWorthToAttackThisPoint(x, y) &&
					x >= 0 && y >= 0 && x < _field.Size && y < _field.Size &&
					_field[x, y] != CellState.Attacked &&
					_field[x, y] != CellState.Destroyed)
					return (x, y);

				// pokud selhalo na kontrole,
				//  provede korekturu jako pri utoceni mimo.
				MissCorection();
			}
		}

		/// <summary>
		/// Urcuje, zda je vhodne utocit na tento bod.
		/// </summary>
		/// <param name="x">X-ova souradnice bodu.</param>
		/// <param name="y">Y-ova souradnice bodu.</param>
		/// <returns>True, pokud je vhodne utocit, jinak false.</returns>
		protected virtual bool IsItWorthToAttackThisPoint(int x, int y)
		{
			return true;
		}

		/// <summary>
		/// Provadi korekturu predikci nasledujici bunky lode v pripade
		/// uspesneho utoku.
		/// </summary>
		/// <param name="x">X-ova souradnice bodu.</param>
		/// <param name="y">Y-ova souradnice bodu.</param>
		private void HitCorrection(int x, int y)
		{
			// pokud drive jsme nemeli zadnou informaci o lodi
			if (_lastAttack is null)
			{
				// vybere smer
				var dirId = GetNextDirectionId(x, y);

				// zaznamena informaci o predpoklade rozmisteni
				//  nasledujici bunky lode.
				_lastAttack = new()
				{
					lastHittenPoint = (x, y),
					directionId = dirId,
					length = 1
				};
			}
			// kdyj uz vime neco, pak proste zaznamena bunku a zvasi delku lode.
			else
			{
				_lastAttack.lastHittenPoint = (x, y);
				_lastAttack.length++;
			}
		}

		/// <summary>
		/// Provadi korekturu predikci nasledujici bunky lode v pripade
		/// marneho utoku.
		/// </summary>
		private void MissCorection()
		{
			// ukonci, pokud nic nevime.
			if (_lastAttack is null)
				return;

			// pokud pokusil utocit druhou bunku lode a to selhalo
			if (_lastAttack.length == 1)
			{
				var (x, y) = _lastAttack.lastHittenPoint;
				var dirId = _lastAttack.directionId;

				// zvoli jiny smer.
				_lastAttack.directionId = GetNextDirectionId(x, y, dirId + 1);
			}
			// pokud jiz porazil 2 a vice bunek lode a ted utok selhal
			else
			{
				// zvoli protilehly smer
				var dirId = GetOppositeDirection(_lastAttack.directionId);

				_lastAttack.directionId = dirId;

				// a nastavi posledni porazeny bod na protilehlou bunku lode.
				var shiftX = _lastAttack.lastHittenPoint.Item1;
				var shiftY = _lastAttack.lastHittenPoint.Item2;

				var dir = _directions[dirId];

				for (int i = 0; i < _lastAttack.length - 1; i++)
				{
					shiftX += dir.Item1;
					shiftY += dir.Item2;
				}

				_lastAttack.lastHittenPoint = (shiftX, shiftY);
			}
		}

		/// <summary>
		/// Smaze daty posledniho utoku.
		/// </summary>
		private void ClearLastAttack()
		{
			_lastAttack = null;
		}

		/// <summary>
		/// Dostane dalsi vhodny smer utoku na zaklade souradnic a stavajiciho smeru.
		/// </summary>
		/// <param name="x">X-ova souradnice bodu.</param>
		/// <param name="y">Y-ova souradnice bodu.</param>
		/// <param name="startDirId">Id stavajiciho smeru.</param>
		/// <returns>Id dalsiho vhodneho smeru.</returns>
		private int GetNextDirectionId(int x, int y, int startDirId = 0)
		{
			int dirId;
			for (dirId = startDirId; dirId < _directions.Length; dirId++)
			{
				var dir = _directions[dirId];

				var shiftX = x + dir.Item1;
				var shiftY = y + dir.Item2;

				// pokud novy smer je mimo pole, pokracuje k nasledujicimu.
				if (shiftX < 0 || shiftX >= _field.Size ||
				   shiftY < 0 || shiftY >= _field.Size)
					continue;

				return dirId;
			}

			return 0;
		}

		/// <summary>
		/// Dostane protilehly smer.
		/// </summary>
		/// <param name="dirId">Id stavajiciho smeru.</param>
		/// <returns>Id protilehleho smeru.</returns>
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

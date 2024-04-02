using SeaBattles.Console.Misc;

#nullable disable

namespace SeaBattles.Console
{
	internal class Engine
	{
		private const string INPUT_SAVE_AND_EXIT = "uloz";

		private const string MSG_SHIP_HIT = "porazeni";
		private const string MSG_SHIP_DESTROYED = "znicena";
		private const string MSG_FIRE_MISSED = "minuti cile";
		private const string MSG_BAD_INPUT = "spatny vstup";

		private readonly BattleField _userField;
		private readonly BattleField _compField;

		private string _attakResultMsg = string.Empty;

		public Engine(BattleField userField, BattleField compField)
		{
			_userField = userField;
			_compField = compField;
		}

		public void Start()
		{
			System.Console.Clear();

			var isPlayerMove = true;

			while (true)
			{
				Draw(isPlayerMove);

				PrintAttackResultMessage();

				if (!TakeMove(isPlayerMove, out var canMoveContinue))
					continue; // spatny vstup

				if (_userField.IsEmpty || _compField.IsEmpty)
					break;

				if (canMoveContinue)
					continue; // pokracuj, pokud rozbivas lode

				Draw(isPlayerMove, true);

				PrintAttackResultMessage();

				PrintContinueMessage();

				isPlayerMove = !isPlayerMove;
			}

			if (isPlayerMove)
				DeclareVictory();
			else
				DeclareDefeat();

			System.Console.WriteLine();
			System.Console.WriteLine();
			System.Console.WriteLine();
			System.Console.WriteLine();

			PrintContinueMessage();
		}

		#region Printings

		private void PrintAttackResultMessage()
		{
			if (string.IsNullOrEmpty(_attakResultMsg))
				return;

			//System.Console.WriteLine();
			System.Console.WriteLine(_attakResultMsg.PadLeft(15));
			System.Console.WriteLine();

			_attakResultMsg = string.Empty;
		}

		private static void PrintContinueMessage()
		{
			System.Console.WriteLine("Pokracujte stiskem libovolne klavesy...");

			System.Console.ReadLine();
		}

		private void Draw(bool isPlayerMove, bool noInputTips = false)
		{
			System.Console.Clear();

			var moveStr = $"==== {(isPlayerMove ? "   Vas tah   " : "Pocitacuv tah")} ====";

			var addStr = (!noInputTips && isPlayerMove ? $"   Pokud chcete ulozit a ukoncit hru, zadejte \'{INPUT_SAVE_AND_EXIT}\'." : string.Empty);

			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '='));
			System.Console.WriteLine(moveStr + addStr);
			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '='));

			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine("Zbyva lodi:");
			System.Console.WriteLine("   Vase          Pocitacove");
			System.Console.WriteLine($"    {_userField.ShipCount,-2}               {_compField.ShipCount, -2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  {(isPlayerMove ? "Pocitacova" : "Vase")} plocha:");
			System.Console.WriteLine();

			if (isPlayerMove)
				BattlefieldDrawer.Draw(_compField, true); // set false only to debug purpose
			else
				BattlefieldDrawer.Draw(_userField, false);

			System.Console.WriteLine();
			System.Console.WriteLine();
		}

		#endregion

		#region Take move

		private bool TakeMove(bool isPlayerMove, out bool canMoveContinue)
		{
			if (isPlayerMove)
			{
				return TakePlayerMove(out canMoveContinue);
			}
			else
			{
				TakeComputerMove(out canMoveContinue);

				return true;
			}
		}

		private bool TakePlayerMove(out bool canMoveContinue)
		{
			canMoveContinue = false;

			var str = System.Console.ReadLine();

			if (str.Equals(INPUT_SAVE_AND_EXIT))
			{
				SaveGameAndExit();
				return false;
			}

			if (!ShipCoordinateParser.TryParse(str, _compField.Size, out var x, out var y))
			{
				_attakResultMsg = MSG_BAD_INPUT;

				return false;
			}

			if (_compField[x, y] == CellState.Attacked ||
				_compField[x, y] == CellState.Destroyed)
			{
				_attakResultMsg = MSG_BAD_INPUT;

				return false;
			}

			var res = _compField.Attack((uint)x, (uint)y);

			switch (res)
			{
				case AttackResult.Failed:
					_attakResultMsg = MSG_BAD_INPUT;

					return false;
				case AttackResult.Missed:
					_attakResultMsg = MSG_FIRE_MISSED;

					return true;
				case AttackResult.Hitten:
					_attakResultMsg = MSG_SHIP_HIT;

					canMoveContinue = true;
					return true;
				case AttackResult.Destroyed:
					_attakResultMsg = MSG_SHIP_DESTROYED;

					canMoveContinue = true;
					return true;
			}

			_attakResultMsg = MSG_BAD_INPUT;
			return false;
		}

		private void TakeComputerMove(out bool canMoveContinue)
		{
			canMoveContinue = false;

			PretendComputerNeedMoreTimeForThinking();

			while (true)
			{
				int x, y;

				while (true)
				{
					var res = ComputerFieldCoordsGenerator.Generate(_userField.Size);
					(x, y) = (res.X, res.Y);

					if (_userField[x, y] == CellState.Attacked ||
						_userField[x, y] == CellState.Destroyed)
						continue;

					break;
				}

				var attackRes = _userField.Attack((uint)x, (uint)y);

				switch (attackRes)
				{
					case AttackResult.Failed:
						_attakResultMsg = MSG_BAD_INPUT;

						return;
					case AttackResult.Missed:
						_attakResultMsg = MSG_FIRE_MISSED;

						return;
					case AttackResult.Hitten:
						_attakResultMsg = MSG_SHIP_HIT;

						canMoveContinue = true;
						return;
					case AttackResult.Destroyed:
						_attakResultMsg = MSG_SHIP_DESTROYED;

						canMoveContinue = true;
						return;
				}
			}
		}

		private static void PretendComputerNeedMoreTimeForThinking()
		{
			var random = new Random();

			var duration = random.Next(1000, 5001);

			Thread.Sleep(duration);
		}


		#endregion

		#region Declarations

		private static void DeclareVictory()
		{
			System.Console.Clear();

			System.Console.WriteLine();

			System.Console.WriteLine("=======================");
			System.Console.WriteLine("====     VYHRA     ====");
			System.Console.WriteLine("=======================");
		}

		private static void DeclareDefeat()
		{
			System.Console.Clear();

			System.Console.WriteLine();

			System.Console.WriteLine("=======================");
			System.Console.WriteLine("====    PORAZKA    ====");
			System.Console.WriteLine("=======================");
		}

		#endregion

		#region Save game

		private void SaveGameAndExit()
		{

		}

		#endregion
	}
}

﻿using SeaBattles.Console.Input;
using SeaBattles.Console.Misc;

namespace SeaBattles.Console.States.Engine
{
	internal class PlayerMoveState : IState
	{
		private const string REGEX_SAVE_AND_EXIT = "^uloz$";
		private const string REGEX_USE_HINT = "^nap$";
		private const string REGEX_MOVE = @"^[a-zA-Z]\s*\d{1,2}$";
		private const string REGEX_EXIT = @"^konc$";

		private readonly Console.Engine _engine;

		private readonly InputHandler _inputHandler;

		public PlayerMoveState(Console.Engine engine)
		{
			_engine = engine;

			_inputHandler = new InputHandler();

			InitInputHandler();
		}

		public void Invoke()
		{
			Draw();

			var input = (System.Console.ReadLine() ?? string.Empty).Trim();

			if (!_inputHandler.Handle(input))
			{
				_engine.StateMsg = Console.Engine.MSG_BAD_INPUT;
			}
		}

		#region Drawing

		private void Draw()
		{
			System.Console.Clear();

			var moveStr = "====    Vas tah    ====";

			var saveTipStr = $"   Pokud chcete ulozit a ukoncit hru, zadejte \'uloz\'.";
			var exitTipStr = "   Pokud chcete ukoncit hru, zadejte \'konc\'.";

			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '='));
			System.Console.WriteLine(moveStr + saveTipStr);
			System.Console.WriteLine(string.Empty.PadRight(moveStr.Length, '=') + exitTipStr);

			System.Console.WriteLine();
			System.Console.WriteLine($"Napovedy: {_engine.LevelData.RemainingHintCount}. Pis \'nap\'");
			System.Console.WriteLine();

			System.Console.WriteLine("Zbyva lodi:");
			System.Console.WriteLine("   Vase          Pocitacove");
			System.Console.WriteLine($"    {_engine.LevelData.UserField.ShipCount,-2}               {_engine.LevelData.CompField.ShipCount,-2}");

			System.Console.WriteLine();
			System.Console.WriteLine($"  Pocitacova plocha:");
			System.Console.WriteLine();

			BattlefieldDrawer.Draw(_engine.LevelData.CompField, true, _engine.LevelData.Hints); // set false only to debug purpose

			System.Console.WriteLine();
			System.Console.WriteLine();

			System.Console.WriteLine(_engine.StateMsg.PadLeft(15));
			System.Console.WriteLine();
		}

		#endregion

		#region Input handlers

		private void TakeMove(string input)
		{
			if (!ShipCoordinateParser.TryParse(input, _engine.LevelData.CompField.Size, out var x, out var y))
			{
				_engine.StateMsg = Console.Engine.MSG_BAD_INPUT;

				//_engine.SetState(new PlayerMoveState(_engine));

				return;
			}

			if (_engine.LevelData.CompField[x, y] == CellState.Attacked ||
				_engine.LevelData.CompField[x, y] == CellState.Destroyed)
			{
				_engine.StateMsg = Console.Engine.MSG_BAD_INPUT;

				//_engine.SetState(new PlayerMoveState(_engine));

				return;
			}

			var res = _engine.LevelData.CompField.Attack((uint)x, (uint)y);

			if (_engine.LevelData.CompField.IsEmpty)
			{
				_engine.SetState(new VictoryState(_engine));

				return;
			}

			switch (res)
			{
				case AttackResult.Failed:
				default:
					_engine.StateMsg = Console.Engine.MSG_BAD_INPUT;

					//_engine.SetState(new PlayerMoveState(_engine));

					return;
				case AttackResult.Missed:
					_engine.StateMsg = Console.Engine.MSG_FIRE_MISSED;

					_engine.SetState(new PlayerMoveResultState(_engine));

					return;
				case AttackResult.Hitten:
					_engine.StateMsg = Console.Engine.MSG_SHIP_HIT;

					//_engine.SetState(new PlayerMoveState(_engine));

					return;
				case AttackResult.Destroyed:
					_engine.StateMsg = Console.Engine.MSG_SHIP_DESTROYED;

					//_engine.SetState(new PlayerMoveState(_engine));

					return;
			}
		}

		private void SaveAndExit()
		{
			_engine.SetState(new SavingState(_engine));
		}

		private void UseHint()
		{
			if (_engine.LevelData.RemainingHintCount <= 0)
				return;

			while (true)
			{
				var hint = _engine.LevelData.CompField.GetRandomShipCell();

				if (hint is null || _engine.LevelData.AddHint(hint.Value))
					break;
			}
			//_engine.SetState(new PlayerMoveState(_engine));
		}

		private void Exit()
		{
			_engine.SetState(null);
		}

		#endregion

		#region Initialization

		private void InitInputHandler()
		{
			_inputHandler.Add(REGEX_MOVE, TakeMove);
			_inputHandler.Add(REGEX_USE_HINT, (_) => { UseHint(); });
			_inputHandler.Add(REGEX_SAVE_AND_EXIT, (_) => { SaveAndExit(); });
			_inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
		}

		#endregion
	}
}

using SeaBattles.Console.FieldFillers;
using SeaBattles.Console.Input;
using SeaBattles.Console.Misc;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
	internal class FieldCreatingState : UserInputState<Game>
	{
		private const string REGEX_SHIP = @"^[a-zA-Z]\s*\d{1,2}\s*[sv]\s*[lmst]$";
		private const string REGEX_CLEAR = "^smaz$";
		private const string REGEX_EXIT = "^konc$";
		private const string REGEX_CONTINUE = "^.*$";

		private readonly FieldSetup _fieldSetup;

		private readonly InputToken _continueToken;
		private Action<InputToken>? AddToken;
		private Action<InputToken>? RemoveToken;

		private FieldFiller _filler;

		public FieldCreatingState(Game game, FieldSetup fieldSetup)
			: base(game)
		{
			_fieldSetup = fieldSetup;

			_filler = FieldFillerFactory.Create(_fieldSetup.Size);

			_continueToken = new(REGEX_CONTINUE, (_) => { BuildField(); });
		}

		#region Drawing

		protected override void Draw()
		{
			if (!_filler.AvailableShips.Any())
				AddToken?.Invoke(_continueToken);

			System.Console.Clear();

			System.Console.WriteLine($"Postupne zadavejte pozice leveho" +
				$" horniho koutu vase lodi, jeji smer " +
				$"a rozmer:");

			System.Console.WriteLine();

			System.Console.WriteLine("Dostupne smery:");
			System.Console.WriteLine($"   Svisla ({ShipCoordinateParser.CHAR_VERTICAL_DIRECTION})");
			System.Console.WriteLine($"   Vodorovna ({ShipCoordinateParser.CHAR_HORIZONTAL_DIRECTION})");

			System.Console.WriteLine();

			System.Console.WriteLine("Dostupne lode:");
			PrintAvailableShips();
			System.Console.WriteLine();

			System.Console.WriteLine($"Priklad: a1 {ShipCoordinateParser.CHAR_VERTICAL_DIRECTION}" +
				$" {ShipCoordinateParser.CHAR_SHIP_LARGE}");

			System.Console.WriteLine();
			_filler.Draw();
			System.Console.WriteLine();

			System.Console.WriteLine("Pokud chcete vycistit plochu, zadejte \'smaz\'.");
			System.Console.WriteLine();

			System.Console.WriteLine("Pokud se chcete vratit do hlavniho menu, zadejte \'konc\'.");

			if (_filler.AvailableShips.Any())
				return;

			System.Console.WriteLine();
			System.Console.WriteLine("Pokracujte stiskem cokoliv jineho.");
		}

		private void PrintAvailableShips()
		{
			System.Console.Write("   ");

			foreach (var pair in _filler.AvailableShips)
			{
				switch (pair.Key)
				{
					case ShipSize.Tiny:
						System.Console.Write($"Maly ({ShipCoordinateParser.CHAR_SHIP_TINY}): {pair.Value};  ");
						break;
					case ShipSize.Small:
						System.Console.Write($"Stredni ({ShipCoordinateParser.CHAR_SHIP_SMALL}): {pair.Value};  ");
						break;
					case ShipSize.Medium:
						System.Console.Write($"Velky ({ShipCoordinateParser.CHAR_SHIP_MEDIUM}): {pair.Value};  ");
						break;
					case ShipSize.Large:
						System.Console.Write($"Obrovsky ({ShipCoordinateParser.CHAR_SHIP_LARGE}): {pair.Value};  ");
						break;
				}
			}

			System.Console.WriteLine();
		}

		#endregion

		#region Input handlers

		private void PutShip(string input)
		{
			if (ShipCoordinateParser.TryParse(input, _filler.Size, out var x, out var y,
					out var direction, out var shipSize))
			{
				_filler.PutShip(x, y, shipSize, direction);
			}
			else
			{
				StateMsg = MSG_BAD_INPUT;
			}
		}

		private void ClearField()
		{
			_filler = FieldFillerFactory.Create(_fieldSetup.Size);

			RemoveToken?.Invoke(_continueToken);
		}

		private void BuildField()
		{
			if (_filler.AvailableShips.Any())
				return;

			var battlefield = _filler.Build();

			SetState(new NewGameState(StateMachine, battlefield, _fieldSetup));
		}

		private void Exit()
		{
			SetState(new MainMenuState(StateMachine));
		}

		#endregion

		#region Initialization

		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
			inputHandler.Add(REGEX_CLEAR, (_) => { ClearField(); });
			inputHandler.Add(REGEX_SHIP, PutShip);

			AddToken = (token) => { inputHandler.Add(token); };
			RemoveToken = (token) => { inputHandler.Remove(token); };
		}

		#endregion
	}
}

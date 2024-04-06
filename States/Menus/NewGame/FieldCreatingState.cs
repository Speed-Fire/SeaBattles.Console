﻿using SeaBattles.Console.FieldFillers;
using SeaBattles.Console.Input;
using SeaBattles.Console.Misc;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
	internal class FieldCreatingState : IState
	{
		private const string REGEX_SHIP = @"^[a-zA-Z]\s*\d{1,2}\s*[sv]\s*[lmst]$";
		private const string REGEX_CLEAR = "^smaz$";
		private const string REGEX_EXIT = "^konc$";
		private const string REGEX_CONTINUE = "^.*$";

		private readonly Game _game;
		private readonly FieldSetup _fieldSetup;

		private readonly InputHandler _inputHandler = new();

		private FieldFiller _filler;

		public FieldCreatingState(Game game, FieldSetup fieldSetup)
		{
			InitInputHandler();
			_game = game;
			_fieldSetup = fieldSetup;

			_filler = FieldFillerFactory.Create(_fieldSetup.Size);
		}

		public void Invoke()
		{
			Draw(_filler);

			var input = (System.Console.ReadLine() ?? string.Empty).Trim();

			_inputHandler.Handle(input);
		}

		#region Drawing

		private static void Draw(FieldFiller filler)
		{
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
			PrintAvailableShips(filler);
			System.Console.WriteLine();

			System.Console.WriteLine($"Priklad: a1 {ShipCoordinateParser.CHAR_VERTICAL_DIRECTION}" +
				$" {ShipCoordinateParser.CHAR_SHIP_LARGE}");

			System.Console.WriteLine();
			filler.Draw();
			System.Console.WriteLine();

			System.Console.WriteLine("Pokud chcete vycistit plochu, zadejte \'smaz\'.");
			System.Console.WriteLine();

			System.Console.WriteLine("Pokud se chcete vratit do hlavniho menu, zadejte \'konc\'.");
			System.Console.WriteLine();

			if (filler.AvailableShips.Any())
				return;

			System.Console.WriteLine();
			System.Console.WriteLine("Pokracujte stiskem cokoliv jineho.");
		}

		private static void PrintAvailableShips(FieldFiller filler)
		{
			System.Console.Write("   ");


			foreach (var pair in filler.AvailableShips)
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
		}

		private void ClearField()
		{
			_filler = FieldFillerFactory.Create(_fieldSetup.Size);
		}

		private void BuildField()
		{
			if (_filler.AvailableShips.Any())
				return;

			var battlefield = _filler.Build();

			_game.SetState(new NewGameState(_game, battlefield, _fieldSetup));
		}

		private void Exit()
		{
			_game.SetState(new MainMenuState(_game));
		}

		#endregion

		#region Initialization

		private void InitInputHandler()
		{
			_inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
			_inputHandler.Add(REGEX_CLEAR, (_) => { ClearField(); });
			_inputHandler.Add(REGEX_SHIP, PutShip);
			_inputHandler.Add(REGEX_CONTINUE, (_) => { BuildField(); });
		}

		#endregion
	}
}
﻿using SeaBattles.Console.Input;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
	internal class FieldSetupState : IState
	{
		private const string REGEX_SIZE = @"^\d+$";
		private const string REGEX_EXIT = @"^konc$";

		private readonly Game _game;
		private readonly InputHandler _inputHandler;

		private const int MIN_FIELD_SIZE = 7;
		private const int MAX_FIELD_SIZE = 15;

		public FieldSetupState(Game game)
		{
			_game = game;
			_inputHandler = new();

			InitInputHandler();
		}

		public void Invoke()
		{
			Draw();

			var input = (System.Console.ReadLine() ?? string.Empty).Trim();

			_inputHandler.Handle(input);
		}

		#region Drawing

		private static void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine("Zadejte velikost plochy:");
			System.Console.WriteLine($"   min: {MIN_FIELD_SIZE};   max: {MAX_FIELD_SIZE}");

			System.Console.WriteLine();
			System.Console.WriteLine("Pokud se chcete vratit do hladniho menu, zadejte \'konc\'.");
			System.Console.WriteLine();
		}

		#endregion

		#region Input handlers

		private void GetSize(string input)
		{
			if (int.TryParse(input, out var size)
					&& size >= MIN_FIELD_SIZE
					&& size <= MAX_FIELD_SIZE)
			{
				_game.SetState(new FieldCreatingState(_game, new FieldSetup(size)));
			}
		}

		private void Exit()
		{
			_game.SetState(new MainMenuState(_game));
		}

		#endregion

		#region Initialization

		private void InitInputHandler()
		{
			_inputHandler.Add(REGEX_SIZE, GetSize);
			_inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
		}

		#endregion
	}
}

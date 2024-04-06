using SeaBattles.Console.FieldFactories;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
	internal class NewGameState : IState
	{
		private readonly Game _game;
		private readonly BattleField _userField;
		private readonly FieldSetup _fieldSetup;

		private readonly IFieldFactory _fieldFactory;

		public NewGameState(Game game, BattleField userField, FieldSetup fieldSetup)
		{
			_game = game;
			_userField = userField;
			_fieldSetup = fieldSetup;

			_fieldFactory = new ComputerFieldFactory();
		}

		public void Invoke()
		{
			var compField = _fieldFactory.CreateBattlefield(_fieldSetup);

			var engine = new Console.Engine(_userField, compField);

			_game.SetState(new PlayingState(_game, engine));
		}
	}
}

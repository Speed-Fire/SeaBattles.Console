using SeaBattles.Console.AI;
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

			var levelData = new LevelData(GetAI(_userField), _userField, compField);

			var engine = new Console.Engine(levelData);

			_game.SetState(new PlayingState(_game, engine));
		}

		private AIPlayer GetAI(BattleField userField)
		{
			return _fieldSetup.Difficult switch
			{
				Difficult.Normal => new AIPlayerNormal(userField),
				Difficult.Hard => new AIPlayerHard(userField),
				_ => new AIPlayerEasy(userField),
			};
		}
	}
}

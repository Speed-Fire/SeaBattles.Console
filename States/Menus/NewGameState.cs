using SeaBattles.Console.AI;
using SeaBattles.Console.FieldFactories;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
	internal class NewGameState : StateBase<Game>
	{
		private readonly BattleField _userField;
		private readonly FieldSetup _fieldSetup;

		private readonly IFieldFactory _fieldFactory;

		public NewGameState(Game game, BattleField userField, FieldSetup fieldSetup)
			: base(game)
		{
			_userField = userField;
			_fieldSetup = fieldSetup;

			_fieldFactory = new ComputerFieldFactory();
		}

		public override void Invoke()
		{
			var compField = _fieldFactory.CreateBattlefield(_fieldSetup);

			var levelData = new LevelData(GetAI(_userField), _userField, compField);

			var engine = new Console.Engine(levelData);

			SetState(new PlayingState(StateMachine, engine));
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

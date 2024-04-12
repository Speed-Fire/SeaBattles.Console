namespace SeaBattles.Console.States.Menus
{
	internal class PlayingState : StateBase<Game>
	{
		private readonly SeaBattles.Console.Engine _engine;
		
		public PlayingState(Game game, SeaBattles.Console.Engine engine)
			: base(game)
		{
			_engine = engine;
		}

		public override void Invoke()
		{
			_engine.Start();

			SetState(new MainMenuState(StateMachine));
		}
	}
}

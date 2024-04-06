namespace SeaBattles.Console.States.Menus
{
	internal class PlayingState : IState
	{
		private readonly Game _game;
		private readonly SeaBattles.Console.Engine _engine;
		
		public PlayingState(Game game, SeaBattles.Console.Engine engine)
		{
			_game = game;

			_engine = engine;
		}
		public void Invoke()
		{
			_engine.Start();

			_game.SetState(new MainMenuState(_game));
		}
	}
}

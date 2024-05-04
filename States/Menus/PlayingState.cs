namespace SeaBattles.Console.States.Menus
{
	/// <summary>
	/// Trida reprezentujici hraci stav hlavniho automatu.
	/// V tomto stavu se spuska herni automat.
	/// </summary>
	internal class PlayingState : StateBase<Game>
	{
		private readonly Console.Engine _engine;
		
		public PlayingState(Game game, Console.Engine engine)
			: base(game)
		{
			_engine = engine;
		}

		/// <summary>
		/// Spusti herni automat.
		/// </summary>
		public override void Invoke()
		{
			_engine.Start();

			// vrati do hlavniho menu, az kra bude ukoncena.
			SetState(new MainMenuState(StateMachine));
		}
	}
}

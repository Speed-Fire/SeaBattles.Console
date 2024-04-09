namespace SeaBattles.Console.States.Engine
{
	internal abstract class EngineState : IState
	{
		protected readonly Console.Engine _engine;
		private readonly Func<string, IState> _stateGetter;

		protected EngineState(Console.Engine engine,
			Func<string, IState> stateGetter)
		{
			_engine = engine;
			_stateGetter = stateGetter;
		}

		public abstract void Invoke();

		protected void SetState(string name)
		{
			_engine.SetState(_stateGetter.Invoke(name));
		}

		protected void SetNullState()
		{
			_engine.SetState(null);
		}
	}
}

namespace SeaBattles.Console.States
{
	internal abstract class StateBase<TStateMachine> : IState
		where TStateMachine : StateMachine
	{
		protected TStateMachine StateMachine { get; }

		protected StateBase(TStateMachine stateMachine)
		{
			StateMachine = stateMachine;
		}

		public abstract void Invoke();

		protected void SetState(StateBase<TStateMachine>? state)
		{
			StateMachine.SetState(state);
		}
	}
}

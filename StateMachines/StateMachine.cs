using SeaBattles.Console.States;

namespace SeaBattles.Console
{
	internal abstract class StateMachine
	{
		protected IState? CurrentState { get; set; }

		public void Start()
		{
			while (CurrentState is not null)
			{
				CurrentState.Invoke();
			}
		}

		public void SetState(IState? state)
		{
			CurrentState = state;
		}
	}
}

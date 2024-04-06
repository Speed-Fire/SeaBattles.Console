using SeaBattles.Console.States;

namespace SeaBattles.Console
{
	internal abstract class StateMachine
	{
		public const string MSG_BAD_INPUT = "spatny vstup";

		protected IState? CurrentState { get; set; }

		private string _stateMsg = string.Empty;
		public string StateMsg
		{
			get
			{
				var tmp = _stateMsg;

				_stateMsg = string.Empty;

				return tmp;
			}

			set
			{
				_stateMsg = value;
			}
		}

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

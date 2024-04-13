using SeaBattles.Console.Input;

namespace SeaBattles.Console.States
{
	internal abstract class UserInputState<TStateMachine> : StateBase<TStateMachine>
		where TStateMachine : StateMachine
	{
		protected const string MSG_BAD_INPUT = "spatny vstup";

		private readonly InputHandler _inputHandler;

		private string _stateMsg = string.Empty;
		protected string StateMsg
		{
			private get
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

		public UserInputState(TStateMachine stateMachine) : base(stateMachine)
		{
			_inputHandler = new();

			StateMsg = string.Empty;

			InitInputHandler(_inputHandler);
		}

		public override void Invoke()
		{
			DrawInternal();

			var input = (System.Console.ReadLine() ?? string.Empty).Trim();

			if (!_inputHandler.Handle(input))
			{
				StateMsg = MSG_BAD_INPUT;
			}
		}

		private void DrawInternal()
		{
			Draw();

			var color = System.Console.ForegroundColor;
			System.Console.ForegroundColor = ConsoleColor.DarkRed;

			System.Console.WriteLine();
			System.Console.WriteLine(StateMsg.PadLeft(15));
			System.Console.WriteLine();

			System.Console.ForegroundColor = color;
		}

		protected abstract void Draw();

		protected abstract void InitInputHandler(InputHandler inputHandler);
	}
}

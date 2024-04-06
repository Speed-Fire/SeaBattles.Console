using SeaBattles.Console.States;
using SeaBattles.Console.States.Menus;

namespace SeaBattles.Console
{
	internal class Game
	{
		private IState? CurrentState { get; set; }

        public Game()
        {
			CurrentState = new MainMenuState(this);
        }

        public void Start()
		{
			while(CurrentState is not null)
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

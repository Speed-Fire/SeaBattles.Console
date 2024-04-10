using SeaBattles.Console.States.Menus;

namespace SeaBattles.Console
{
	internal class Game : StateMachine
	{
        public Game()
        {
			CurrentState = new MainMenuState(this);
        }
	}
}

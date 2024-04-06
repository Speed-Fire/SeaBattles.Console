using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattles.Console.States.Menus
{
	internal class LoadGameState : IState
	{
		private readonly Game _game;
		
		public LoadGameState(Game game)
		{
			_game = game;
		}
		public void Invoke()
		{
			throw new NotImplementedException();
		}
	}
}

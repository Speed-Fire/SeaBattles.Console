using SeaBattles.Console.Models;
using SeaBattles.Console.States.Engine;

namespace SeaBattles.Console
{
	internal class Engine : StateMachine
	{
		public const string MSG_SHIP_HIT = "porazeni";
		public const string MSG_SHIP_DESTROYED = "znicena";
		public const string MSG_FIRE_MISSED = "minuti cile";

		public LevelData LevelData { get; }

		public Engine(LevelData levelData)
		{
			LevelData = levelData;

			SetState(new PlayerMoveState(this));
		}
	}
}

namespace SeaBattles.Console.States.Game
{
	internal class VictoryState : IState
	{
		private readonly Engine _engine;

		public VictoryState(Engine engine)
		{
			_engine = engine;
		}

		public void Invoke()
		{
			Draw();

			System.Console.ReadLine();

			_engine.SetState(null);
		}

		#region Drawing

		private static void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine();

			System.Console.WriteLine("=======================");
			System.Console.WriteLine("====     VYHRA     ====");
			System.Console.WriteLine("=======================");
		}

		#endregion
	}
}

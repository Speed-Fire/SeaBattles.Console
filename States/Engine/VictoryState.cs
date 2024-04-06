namespace SeaBattles.Console.States.Engine
{
	internal class VictoryState : IState
	{
		private readonly Console.Engine _engine;

		public VictoryState(Console.Engine engine)
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

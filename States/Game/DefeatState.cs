namespace SeaBattles.Console.States.Game
{
	internal class DefeatState : IState
	{
		private readonly Engine _engine;

		public DefeatState(Engine engine)
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
			System.Console.WriteLine("====    PORAZKA    ====");
			System.Console.WriteLine("=======================");
		}

		#endregion
	}
}

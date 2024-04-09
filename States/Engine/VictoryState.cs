
namespace SeaBattles.Console.States.Engine
{
	internal class VictoryState : EngineState
	{
		public VictoryState(Console.Engine engine, Func<string, IState> stateGetter) 
			: base(engine, stateGetter)
		{
		}

		public override void Invoke()
		{
			Draw();

			System.Console.ReadLine();

			SetNullState();
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

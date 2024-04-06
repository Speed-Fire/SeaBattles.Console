namespace SeaBattles.Console
{
	internal class Program
	{
		static void Main(string[] args)
		{
			PrepareConsole();

			var game = new Game();

			game.Start();
		}

		private static void PrepareConsole()
		{
			System.Console.OutputEncoding = System.Text.Encoding.Unicode;

			System.Console.SetWindowSize(System.Console.LargestWindowWidth,
				System.Console.LargestWindowHeight);

			System.Console.Title = "Sea battles";

			System.Console.SetWindowPosition(0, 0);
		}
	}
}

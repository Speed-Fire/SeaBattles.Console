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

			System.Console.Title = "Hra lode";
		}
	}
}

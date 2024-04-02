using SeaBattles.Console.GameInitializers;

namespace SeaBattles.Console
{
	internal enum MainMenuOption
	{
		NewGame = 0,
		LoadGame,
		Exit
	}

	internal class Program
	{
		static void Main(string[] args)
		{
			PrepareConsole();

			while (true)
			{
				var option = MainMenu();

				switch (option)
				{
					case MainMenuOption.NewGame:
						StartGame(new NewGameEngineFactory());
						break;
					case MainMenuOption.LoadGame:
						StartGame(new LoadGameEngineFactory());
						break;
					case MainMenuOption.Exit:
						return;
				}
			}
		}

		private static MainMenuOption MainMenu()
		{
			do
			{
				System.Console.Clear();

				System.Console.WriteLine("Vitam Vas ve hre Lode!");
				System.Console.WriteLine("Zvolte prosim cislo pro pokracovani:");
				System.Console.WriteLine("1. Nova hra");
				System.Console.WriteLine("2. Nahrat hru");
				System.Console.WriteLine("3. Staci");

				var res = System.Console.ReadLine();

				if (int.TryParse(res, out var option) &&
					option > 0 &&
					option <= Enum.GetValues(typeof(MainMenuOption)).Length)
				{
					return (MainMenuOption)(option - 1);
				}
			}
			while (true);
		}

		private static void StartGame(IEngineFactory engineFactory)
		{
			var engine = engineFactory.CreateEngine();

			engine.Start();
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

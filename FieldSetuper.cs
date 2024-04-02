using SeaBattles.Console.Models;

namespace SeaBattles.Console
{
	internal class FieldSetuper
	{
		private const int MIN_FIELD_SIZE = 7;
		private const int MAX_FIELD_SIZE = 15;

		public static FieldSetup GetSetup()
		{
			var size = GetSize();

			return new FieldSetup(size);
		}

		private static int GetSize()
		{
			do
			{
				Print();

				var str = System.Console.ReadLine();

				if (int.TryParse(str, out var size)
					&& size >= MIN_FIELD_SIZE
					&& size <= MAX_FIELD_SIZE)
				{
					return size;
				}
			} while (true);
		}

		private static void Print()
		{
			System.Console.Clear();

			System.Console.WriteLine("Zadejte velikost plochy:");
			System.Console.WriteLine($"   min: {MIN_FIELD_SIZE};   max: {MAX_FIELD_SIZE}");
		}
	}
}

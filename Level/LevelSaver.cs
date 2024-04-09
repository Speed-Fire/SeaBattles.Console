using SeaBattles.Console.AI;
using SeaBattles.Console.Models;
using System.Text;

namespace SeaBattles.Console.Level
{
	internal class LevelSaver
	{
		private static readonly string _directory = Path.Combine(Directory.GetCurrentDirectory(), "Saves");

		public static bool Save(LevelData levelData)
		{
			var sb = new StringBuilder();

			var dt = DateTime.Now;
			dt = dt.AddMilliseconds(-dt.Millisecond);

			try
			{
				CreateHeader(levelData, sb, dt);
				sb.AppendLine();
				CreateBody(levelData, sb);

				string path = GetFilePath(dt);

				File.WriteAllText(path, sb.ToString());

				return true;
			}
			catch
			{
				return false;
			}
		}

		private static string GetFilePath(DateTime dt)
		{
			var dirpath = _directory;
			if (!Directory.Exists(dirpath))
				Directory.CreateDirectory(dirpath);

			var filename = dt.ToString("yyyy.MM.dd HH.mm.ss")
				.Replace(".", string.Empty)
				.Replace(" ", string.Empty);

			var path = Path.Combine(dirpath, filename);
			return path;
		}

		private static void CreateHeader(LevelData levelData, StringBuilder sb, DateTime dt)
		{
			sb.AppendLine(dt.ToString("yyyy.MM.dd HH:mm:ss"))
			  .Append(levelData.AI switch
			  {
				  AIPlayerEasy => 0,
				  AIPlayerHard => 2,
				  AIPlayerNormal => 1,
				  _ => throw new InvalidOperationException("Unsupported AI")
			  })
			  .Append(' ')
			  .Append(levelData.UserField.Size)
			  .Append(Environment.NewLine);
		}

		private static void CreateBody(LevelData levelData, StringBuilder sb)
		{
			sb.AppendLine(levelData.AI.ToString());
			sb.AppendLine();
			sb.AppendLine(levelData.UserField.ToString());
			sb.AppendLine();
			sb.AppendLine(levelData.CompField.ToString());
		}
	}
}

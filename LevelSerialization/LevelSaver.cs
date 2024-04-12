using SeaBattles.Console.Models;

namespace SeaBattles.Console.Level
{
	internal record LevelSave(LevelHeader Header, DateTime Creaated, string Path);

	internal class LevelSaver
	{
		private static readonly string DIRECTORY = Path.Combine(Directory.GetCurrentDirectory(), "Saves");

		private const string DATETIME_FORMAT = "yyyy.MM.dd HH:mm:ss";

		private const string SAVE_NAME_TEMPLATE = "seabattle";
		private const string SAVE_NAME_SEARCH_PATTERN = $"{SAVE_NAME_TEMPLATE}*";

		private const int MAX_SAVE_COUNT = 3;

		public static bool Save(LevelData levelData)
		{
			var filename = GetFilePath(DateTime.Now);

			var res = LevelSerializer.Serialize(levelData, filename);

			if (res)
				RemoveOldSaves();

			return res;
		}

		public static IEnumerable<LevelSave> GetSaves()
		{
			var saves = new List<LevelSave>();

			foreach (var file in Directory.EnumerateFiles(DIRECTORY, SAVE_NAME_SEARCH_PATTERN))
			{
				var header = LevelSerializer.DeserializeHeader(file);

				if (header is null)
					continue;

				var datetime = File.GetLastWriteTime(file);

				saves.Add(new(header, datetime, file));
			}

			return saves;
		}

		public static LevelData? Load(LevelSave levelSave)
		{
			return LevelSerializer.Deserialize(levelSave.Path);
		}

		public static bool Remove(LevelSave levelSave)
		{
			try
			{
				File.Delete(levelSave.Path);

				return true;
			}
			catch 
			{
				return false;
			}
		}

		#region Internal

		private static string GetFilePath(DateTime dt)
		{
			var dirpath = DIRECTORY;
			if (!Directory.Exists(dirpath))
				Directory.CreateDirectory(dirpath);

			var filename = SAVE_NAME_TEMPLATE + dt.ToString(DATETIME_FORMAT)
				.Replace(".", string.Empty)
				.Replace(" ", string.Empty)
				.Replace(":", string.Empty);

			var path = Path.Combine(dirpath, filename);
			return path;
		}

		private static void RemoveOldSaves()
		{
			var files = Directory.GetFiles(DIRECTORY, SAVE_NAME_SEARCH_PATTERN);

			if (files.Length <= MAX_SAVE_COUNT)
				return;

			var data = files
				.Select(x => (Path: x, Time: File.GetLastWriteTime(x)))
				.OrderBy(x => x.Time);

			var count = data.Count();

			foreach(var file in data.Take(count - MAX_SAVE_COUNT))
			{
				File.Delete(file.Path);
			}
		}

		#endregion
	}
}

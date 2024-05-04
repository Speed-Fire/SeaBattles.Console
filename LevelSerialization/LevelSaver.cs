using SeaBattles.Console.Models;

namespace SeaBattles.Console.Level
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	/// <summary>
	/// Zaznam urovne obsahujici zahlavi urovne, datum vytvoreni a cestu k souboru.
	/// </summary>
	internal record LevelSave(LevelHeader Header, DateTime Created, string Path);

	/// <summary>
	/// Trida pro ukladani a nacitani urovni.
	/// </summary>
	internal class LevelSaver
	{
		private static readonly string DIRECTORY = Path.Combine(Directory.GetCurrentDirectory(), "Saves");

		private const string DATETIME_FORMAT = "yyyy.MM.dd HH:mm:ss";

		private const string SAVE_NAME_TEMPLATE = "seabattle";
		private const string SAVE_NAME_SEARCH_PATTERN = $"{SAVE_NAME_TEMPLATE}*";

		private const int MAX_SAVE_COUNT = 9;

		/// <summary>
		/// Ulozi uroven.
		/// </summary>
		/// <param name="levelData">Data urovne k ulozeni.</param>
		/// <returns>True, pokud se uroven uspesne ulozila; jinak false.</returns>
		public static bool Save(LevelData levelData)
		{
			var filename = GetFilePath(DateTime.Now);

			var res = LevelSerializer.Serialize(levelData, filename);

			if (res)
				RemoveOldSaves();

			return res;
		}

		/// <summary>
		/// Ziska seznam ulozenych urovni.
		/// </summary>
		/// <returns>Seznam ulozenych urovni.</returns>
		public static IEnumerable<LevelSave> GetSaves()
		{
			var saves = new List<LevelSave>();

			// vytvori slozku, pokud neexistuje.
			var dirpath = DIRECTORY;
			if (!Directory.Exists(dirpath))
				Directory.CreateDirectory(dirpath);

			// nalezne vsechny vyhovujici soubory, ktere mohou byt zaznamy her.
			foreach (var file in Directory.EnumerateFiles(DIRECTORY, SAVE_NAME_SEARCH_PATTERN))
			{
				var header = LevelSerializer.DeserializeHeader(file);

				// pokud nepodarilo se nacist hlavicku zaznamu, pokracuje dal
				if (header is null)
					continue;

				var datetime = File.GetLastWriteTime(file);

				saves.Add(new LevelSave(header, datetime, file));
			}

			return saves;
		}

		/// <summary>
		/// Nacte ulozenou uroven.
		/// </summary>
		/// <param name="levelSave">Uroven k nacteni.</param>
		/// <returns>Data nactene urovne nebo null, pokud se nacteni nezdarelo.</returns>
		public static LevelData? Load(LevelSave levelSave)
		{
			return LevelSerializer.Deserialize(levelSave.Path);
		}

		/// <summary>
		/// Odebere ulozenou uroven.
		/// </summary>
		/// <param name="levelSave">Uroven k odebrani.</param>
		/// <returns>True, pokud se uroven uspesne odebrala; jinak false.</returns>
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

		/// <summary>
		/// Ziska cestu k souboru pro ulozeni urovne na zaklade aktualniho data a casu.
		/// </summary>
		/// <param name="dt">Datum a cas pro zahrnuti do nazvu souboru.</param>
		/// <returns>Cesta k souboru pro ulozeni urovne.</returns>
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

		/// <summary>
		/// Smaze stare ulozene zaznamy, pokud jich je vice nez povoleny pocet.
		/// POZOR: budou smazany vsechny soubory s vyhovujicim jmenem, dokonce kdyz nejsou
		/// zaznamy hry.
		/// </summary>
		private static void RemoveOldSaves()
		{
			var files = Directory.GetFiles(DIRECTORY, SAVE_NAME_SEARCH_PATTERN);

			// pokud maximalni pocet zaznamu neni prekrocen,
			//  nic mazat nebude.
			if (files.Length <= MAX_SAVE_COUNT)
				return;

			var data = files
				.Select(x => (Path: x, Time: File.GetLastWriteTime(x)))
				.OrderBy(x => x.Time);

			var count = data.Count();

			foreach (var file in data.Take(count - MAX_SAVE_COUNT))
			{
				File.Delete(file.Path);
			}
		}

		#endregion
	}

}

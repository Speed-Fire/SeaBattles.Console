using SeaBattles.Console.AI;
using SeaBattles.Console.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace SeaBattles.Console.Level
{
	internal class LevelLoader
	{
		private static readonly string _directory = Path.Combine(Directory.GetCurrentDirectory(), "Saves");

		public static LevelHeader[] GetLevelHeaders()
		{
			var files = Directory.GetFiles(_directory);
			var headers = new List<LevelHeader>();

			foreach (var file in files)
			{
				try
				{
					var header = GetLevelHeader(file);

					headers.Add(header);
				}
				catch { }
			}

			return headers.ToArray();
		}

		private static LevelHeader GetLevelHeader(string path)
		{
			using var stream = File.OpenRead(path);
			var reader = new StreamReader(stream);

			// get datetime
			var dt = DateTime.Parse(reader.ReadLine().Trim());

			var vals = reader.ReadLine().Trim().Split(' ');

			var difficulty = (Difficult)int.Parse(vals[0]);
			var fieldSize = int.Parse(vals[1]);

			return new(dt, difficulty, fieldSize, path);
		}

#nullable enable

		public static LevelData? LoadLevel(LevelHeader levelHeader)
		{
			try
			{
				using var stream = File.OpenRead(levelHeader.Path);
				var reader = new StreamReader(stream);

				// skip header
				for (int i = 0; i < 2; i++) reader.ReadLine();
				reader.ReadLine();

				// skip or load AI data
				AIPlayerNormal.LastLucklyAttack? aiData = null;
				if (levelHeader.Difficulty == Difficult.Easy)
					reader.ReadLine();
				else
					aiData = GetAIData(reader);

				// load user battlefield
				var userField = LoadField(reader);

				// load comp battlefield
				var compField = LoadField(reader);

				throw new NotImplementedException();
				//return new()
			}
			catch
			{
				return null;
			}
		}

#nullable disable

		private static AIPlayerNormal.LastLucklyAttack GetAIData(StreamReader reader)
		{
			var data = reader.ReadLine();

			var vals = data.Trim().Split(' ').Select(x => int.Parse(x)).ToArray();

			return new()
			{
				lastHittenPoint = (vals[0], vals[1]),
				directionId = vals[2],
				length = vals[3]
			};
		}

		private static BattleField LoadField(StreamReader reader)
		{
			throw new NotImplementedException();
		}
	}
}

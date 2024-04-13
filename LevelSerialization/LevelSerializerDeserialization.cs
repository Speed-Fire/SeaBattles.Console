using SeaBattles.Console.AI;
using SeaBattles.Console.FieldFillers;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.Level
{
	internal record LevelHeader(/*DateTime DateTime,*/
		int Size, Difficulty Difficulty);

	internal partial class LevelSerializer
	{
		private class HintsData
		{
			public int RemainingHintCount { get; }
			public List<(int, int)> Hints { get; }

			public HintsData(int remainingHintCount, List<(int, int)> hints)
			{
				RemainingHintCount = remainingHintCount;
				Hints = hints;
			}
		}

		public static LevelData? Deserialize(string path)
		{
			try
			{
				var blocks = GetBlocks(path);

				var header = ReadHeaderBlock(blocks[BLOCK_HEADER]);

				var hints  = ReadHintsBlock(blocks[BLOCK_HINTS]);

				var aiData = ReadAIBlock(blocks[BLOCK_AI]);

				var uField = ReadBattleFieldBlock(blocks[BLOCK_USER_FIELD]);

				var cField = ReadBattleFieldBlock(blocks[BLOCK_COMP_FIELD]);

				AIPlayer ai = header.Difficulty switch
				{
					Difficulty.Easy => new AIPlayerEasy(uField),
					Difficulty.Normal => new AIPlayerNormal(uField, aiData),
					Difficulty.Hard => new AIPlayerHard(uField, aiData),
					_ => throw new Exception()
				};

				return new(ai, uField, cField, hints.Hints, hints.RemainingHintCount);
			}
			catch
			{
				return null;
			}
		}

		public static LevelHeader? DeserializeHeader(string path)
		{
			try
			{
				var blocks = GetBlocks(path);

				var header = ReadHeaderBlock(blocks[BLOCK_HEADER]);

				return header;
			}
			catch
			{
				return null;
			}
		}

		#region Parsing

		#region Block data getting

		private static Dictionary<string, string> GetBlocks(string path)
		{
			var strs = File.ReadAllText(path).Split(BLOCK_START,
				StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

			var blocks = new Dictionary<string, string>();

			foreach(var str in strs)
			{
				var firstWhitespace = str.IndexOfAny(new char[] { ' ', '\n' });
				var blockName = str.Substring(0, firstWhitespace).Trim();

				switch(blockName)
				{
					case BLOCK_HEADER:
					case BLOCK_AI:
					case BLOCK_USER_FIELD:
					case BLOCK_COMP_FIELD:
					case BLOCK_HINTS:
						break;
					default:
						throw new Exception();
				}

				blocks[blockName] = str.Substring(firstWhitespace).Trim();
			}

			return blocks;
		}

		private static Dictionary<string, string> GetBlockData(string block)
		{
			var data = new Dictionary<string, string>();

			block = block.ReplaceLineEndings("\n");

			var lines = block.Split('\n',
				StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

			foreach(var line in lines)
			{
				var vals = line.Split(':', StringSplitOptions.TrimEntries);

				if (vals.Length != 2)
					throw new Exception();

				data.Add(vals[0], vals[1]);
			}

			return data;
		}

		#endregion

		#region Block readings

		private static LevelHeader ReadHeaderBlock(string block)
		{
			var data = GetBlockData(block);

			//var datetime = DateTime.Parse(data[BLOCK_HEADER_DATETIME]);
			var difficulty = (Difficulty)int.Parse(data[BLOCK_HEADER_DIFFICULTY]);
			var size = int.Parse(data[BLOCK_HEADER_SIZE]);

			return new(/*datetime, */size, difficulty);
		}

		private static AIPlayerNormal.LastLucklyAttack? ReadAIBlock(string block)
		{
			var data = GetBlockData(block);

			var hasData = bool.Parse(data[BLOCK_AI_HAS_DATA]);

			if (!hasData)
				return null;

			var point = data[BLOCK_AI_LAST_HITTEN_POINT].Trim().Split(' ');
			var lastHittenPoint = (int.Parse(point[0]), int.Parse(point[1]));
			var directionId = int.Parse(data[BLOCK_AI_DIRECTION_ID]);
			var length = int.Parse(data[BLOCK_AI_LENGTH]);

			return new()
			{
				lastHittenPoint = lastHittenPoint,
				directionId = directionId,
				length = length
			};
		}

		private static BattleField ReadBattleFieldBlock(string block)
		{
			var data = GetBlockData(block);

			var size = int.Parse(data[BLOCK_FIELD_SIZE]);
			var vals = data[BLOCK_FIELD_FIELD]
				.Trim()
				.Split(' ')
				.Select(x => int.Parse(x))
				.ToList();

			var field = new CellState[size, size];

			for(int i = 0; i < size; i++)
			{
				for(int j = 0; j < size; j++)
				{
					field[i, j] = (CellState)vals[i * size + j];
				}
			}

			var (shipCountField, ships) = FieldShipCalculator.CreateShipCountField(size, field);

			return new(size, field, shipCountField, ships);
		}

		private static HintsData ReadHintsBlock(string block)
		{
			var data = GetBlockData(block);

			var remaining = int.Parse(data[BLOCK_HINTS_REMAINING]);

			var hints_str = data[BLOCK_HINTS_HINTS].Split(' ',
				StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

			var hints = new List<(int, int)>();

			foreach(var hint in hints_str)
			{
				var point = hint.Split('|').Select(x => int.Parse(x)).ToList();

				hints.Add((point[0], point[1]));
			}

			return new(remaining, hints);
		}

		#endregion

		#endregion
	}
}

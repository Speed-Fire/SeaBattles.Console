using SeaBattles.Console.AI;
using SeaBattles.Console.Models;
using System.Text;

namespace SeaBattles.Console.Level
{
	internal partial class LevelSerializer
	{
		public static bool Serialize(LevelData levelData, string path)
		{
			var sb = new StringBuilder();

			try
			{
				CreateHeaderBlock(levelData, sb);
				CreateHintsBlock(levelData, sb);
				CreateAIBlock(levelData, sb);
				CreateUserFieldBlock(levelData, sb);
				CreateCompFieldBlock(levelData, sb);

				File.WriteAllText(path, sb.ToString());

				return true;
			}
			catch
			{
				return false;
			}
		}

		#region Blocks

		#region Header

		private static void CreateHeaderBlock(LevelData levelData, StringBuilder sb)
		{
			sb.AppendLine($"{BLOCK_START} {BLOCK_HEADER}")
			  //.AppendLine($"{BLOCK_HEADER_DATETIME}: {dt.ToString(DATETIME_FORMAT)}")
			  .AppendLine($@"{BLOCK_HEADER_DIFFICULTY}: {levelData.AI switch
			  {
				  AIPlayerEasy => 0,
				  AIPlayerHard => 2,
				  AIPlayerNormal => 1,
				  _ => throw new InvalidOperationException("Unsupported AI")
			  }}")
			  .AppendLine($"{BLOCK_HEADER_SIZE}: {levelData.UserField.Size}")
			  .AppendLine();
		}

		#endregion

		#region Battlefields

		private static void CreateFieldBlock(BattleField field, StringBuilder sb, string blockName)
		{
			sb.AppendLine(blockName)
			  .AppendLine($"{BLOCK_FIELD_SIZE}: {field.Size}")
			  .Append($"{BLOCK_FIELD_FIELD}: ");

			for(int i = 0; i < field.Size; i++)
			{
				for(int j = 0; j < field.Size; j++)
				{
					sb.Append((int)field[i, j])
					  .Append(' ');
				}
			}

			sb.Append(Environment.NewLine)
			  .AppendLine();
		}

		private static void CreateUserFieldBlock(LevelData levelData, StringBuilder sb)
		{
			CreateFieldBlock(levelData.UserField, sb, $"{BLOCK_START} {BLOCK_USER_FIELD}");
		}

		private static void CreateCompFieldBlock(LevelData levelData, StringBuilder sb)
		{
			CreateFieldBlock(levelData.CompField, sb, $"{BLOCK_START} {BLOCK_COMP_FIELD}");
		}

		#endregion

		#region AI

		private static void CreateAIBlock(LevelData levelData, StringBuilder sb)
		{
			AIPlayerNormal.LastLucklyAttack? data = null;

			if (levelData.AI as AIPlayerNormal is not null)
				data = ((AIPlayerNormal)levelData.AI).LastAttack;

			sb.AppendLine($"{BLOCK_START} {BLOCK_AI}")
			  .AppendLine($"{BLOCK_AI_HAS_DATA}: {data is not null}");

			if (data is null)
			{
				sb.AppendLine();
				return;
			}

			sb.AppendLine($"{BLOCK_AI_LAST_HITTEN_POINT}: {data.lastHittenPoint.Item1} {data.lastHittenPoint.Item2}")
			  .AppendLine($"{BLOCK_AI_DIRECTION_ID}: {data.directionId}")
			  .AppendLine($"{BLOCK_AI_LENGTH}: {data.length}")
			  .AppendLine();
		}

		#endregion

		#region Hints

		private static void CreateHintsBlock(LevelData levelData, StringBuilder sb)
		{
			sb.AppendLine($"{BLOCK_START} {BLOCK_HINTS}")
			  .AppendLine($"{BLOCK_HINTS_REMAINING}: {levelData.RemainingHintCount}")
			  .Append($"{BLOCK_HINTS_HINTS}: ");

			foreach(var hint in levelData.Hints)
			{
				sb.Append($"{hint.Item1}|{hint.Item2} ");
			}

			sb.Append(Environment.NewLine)
			  .AppendLine();
		}

		#endregion

		#endregion
	}
}

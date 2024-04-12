namespace SeaBattles.Console.Level
{
	internal partial class LevelSerializer
	{
		private static readonly string DIRECTORY = Path.Combine(Directory.GetCurrentDirectory(), "Saves");

		private const string DATETIME_FORMAT = "yyyy.MM.dd HH:mm:ss";

		private const string SAVE_NAME_TEMPLATE = "seabattle";
		private const string SAVE_NAME_SEARCH_PATTERN = $"{SAVE_NAME_TEMPLATE}*";

		private const string BLOCK_START = "==>";
		private const string BLOCK_HEADER = "HEADER";
		private const string BLOCK_AI = "AI";
		private const string BLOCK_HINTS = "HINTS";
		private const string BLOCK_USER_FIELD = "USER_FIELD";
		private const string BLOCK_COMP_FIELD = "COMP_FIELD";

		private const string BLOCK_HEADER_DATETIME = "DateTime";
		private const string BLOCK_HEADER_DIFFICULTY = "Difficulty";
		private const string BLOCK_HEADER_SIZE = "Size";

		private const string BLOCK_AI_HAS_DATA = "HasData";
		private const string BLOCK_AI_LAST_HITTEN_POINT = "LastHittenPoint";
		private const string BLOCK_AI_DIRECTION_ID = "DirectionId";
		private const string BLOCK_AI_LENGTH = "Length";

		private const string BLOCK_HINTS_REMAINING = "Remaining";
		private const string BLOCK_HINTS_HINTS = "Hints";

		private const string BLOCK_FIELD_SIZE = "Size";
		private const string BLOCK_FIELD_FIELD = "FIELD";
	}
}

using System.Text.RegularExpressions;

namespace SeaBattles.Console.Input
{
	internal class InputToken
	{
		private readonly Regex _regex;

		public Action<string> Action { get; }

		public InputToken(string regex, Action<string> action)
		{
			_regex = new Regex(regex);
			Action = action;
		}

		public bool DoIfMatch(string input)
		{
			if (!_regex.IsMatch(input))
				return false;

			Action.Invoke(input);

			return true;
		}
	}
}

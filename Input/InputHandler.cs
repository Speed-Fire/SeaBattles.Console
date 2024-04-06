namespace SeaBattles.Console.Input
{
	internal class InputHandler
	{
		private readonly List<InputToken> _tokens = new();

		public void Add(InputToken token)
		{
			_tokens.Add(token);
		}

		public void Add(string regex, Action<string> action)
		{
			Add(new InputToken(regex, action));
		}

		public void Remove(InputToken token)
		{
			_tokens.Remove(token);
		}

		public bool Handle(string input)
		{
			foreach (var token in _tokens)
			{
				if (token.DoIfMatch(input))
					return true;
			}

			return false;
		}
	}
}

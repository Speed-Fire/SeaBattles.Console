namespace SeaBattles.Console.Input
{
    /// <summary>
    /// Trida pro zpracovani vstupu pomoci definovanych tokenu.
    /// </summary>
    internal class InputHandler
    {
        private readonly List<InputToken> _tokens = new();

        /// <summary>
        /// Prida novy token do zpracovani vstupu.
        /// </summary>
        /// <param name="token">Token k pridani.</param>
        public void Add(InputToken token)
        {
            _tokens.Add(token);
        }

        /// <summary>
        /// Prida novy token na zaklade regularniho vyrazu a akce.
        /// </summary>
        /// <param name="regex">Regularni vyraz pro shodu s vstupem.</param>
        /// <param name="action">Akce, ktera se ma provest.</param>
        public void Add(string regex, Action<string> action)
        {
            Add(new InputToken(regex, action));
        }

        /// <summary>
        /// Odebere dany token ze zpracovani vstupu.
        /// </summary>
        /// <param name="token">Token k odebrani.</param>
        public void Remove(InputToken token)
        {
            _tokens.Remove(token);
        }

        /// <summary>
        /// Zpracuje vstup pomoci definovanych tokenu.
        /// </summary>
        /// <param name="input">Vstup k zpracovani.</param>
        /// <returns>True, pokud byl vstup uspesne zpracovan nejakym tokenem, jinak false.</returns>
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

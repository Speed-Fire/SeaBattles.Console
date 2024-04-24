using System.Text.RegularExpressions;

namespace SeaBattles.Console.Input
{
    /// <summary>
    /// Trida reprezentujici token pro zpracovani vstupu.
    /// </summary>
    internal class InputToken
    {
        private readonly Regex _regex;

        /// <summary>
        /// Akce, ktera se ma provest, pokud vstup odpovida regularnimu vyrazu.
        /// </summary>
        public Action<string> Action { get; }

        /// <summary>
        /// Konstruktor tridy InputToken.
        /// </summary>
        /// <param name="regex">Regularni vyraz pro shodu s vstupem.</param>
        /// <param name="action">Akce, ktera se ma provest.</param>
        public InputToken(string regex, Action<string> action)
        {
            _regex = new Regex(regex);
            Action = action;
        }

        /// <summary>
        /// Provede akci, pokud vstup odpovida regularnimu vyrazu.
        /// </summary>
        /// <param name="input">Vstup, ktery se ma zpracovat.</param>
        /// <returns>True, pokud byla akce provedena, jinak false.</returns>
        public bool DoIfMatch(string input)
        {
            if (!_regex.IsMatch(input))
                return false;

            Action.Invoke(input);

            return true;
        }
    }
}

using SeaBattles.Console.FieldFactories;
using SeaBattles.Console.FieldFillers;
using SeaBattles.Console.Input;
using SeaBattles.Console.Misc;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
    /// <summary>
    /// Stav tvorby hraciho pole.
    /// </summary>
    internal class FieldCreatingState : UserInputState<Game>
    {
        // Regex pro validaci lodi
        private readonly string REGEX_SHIP = @"^[a-zA-Z]\s*\d{1,2}\s*[sv]\s*[" + 
            ShipCoordinateParser.CHAR_SHIP_TINY +
			ShipCoordinateParser.CHAR_SHIP_SMALL +
			ShipCoordinateParser.CHAR_SHIP_MEDIUM +
			ShipCoordinateParser.CHAR_SHIP_LARGE + "]$";
        private const string REGEX_CLEAR = "^smaz$";
        private const string REGEX_EXIT = "^konc$";
        private const string REGEX_AUTO = "^auto$";
        private const string REGEX_CONTINUE = "^.*$";

        private readonly IFieldFactory _fieldFactory = new ComputerFieldFactory();

        // nastaveni pole.
        private readonly FieldSetup _fieldSetup;

        private readonly InputToken _continueToken;

        // metody pro pridani a odstraneni tokenu osetreni vstupu
        //  po ukoncene inicializace.
        private Action<InputToken>? AddToken;
        private Action<InputToken>? RemoveToken;

        // trida pro plneni pole.
        private FieldFiller _filler;

        /// <summary>
        /// Konstruktor pro stav tvorby pole.
        /// </summary>
        /// <param name="game">Hra.</param>
        /// <param name="fieldSetup">Nastaveni pole.</param>
        public FieldCreatingState(Game game, FieldSetup fieldSetup)
            : base(game)
        {
            _fieldSetup = fieldSetup;

            _filler = FieldFillerFactory.Create(_fieldSetup.Size);

            _continueToken = new(REGEX_CONTINUE, (_) => { BuildField(); });
        }

        #region Drawing

        /// <summary>
        /// Metoda pro vykresleni stavu tvorby pole.
        /// </summary>
        protected override void Draw()
        {
            // pokud vsechny lode jsou rozmisteny na poli,
            //  povoli uzivatelovi pokracovat dal.
            if (!_filler.AvailableShips.Any())
                AddToken?.Invoke(_continueToken);

            System.Console.Clear();
            System.Console.WriteLine($"Postupne zadavejte pozice leveho" +
                $" horniho rohu vasi lodi, jeji smer " +
                $"a rozmer:");
            System.Console.WriteLine();
            System.Console.WriteLine("Dostupne smery:");
            System.Console.WriteLine($"   Svisla ({ShipCoordinateParser.CHAR_VERTICAL_DIRECTION})");
            System.Console.WriteLine($"   Vodorovna ({ShipCoordinateParser.CHAR_HORIZONTAL_DIRECTION})");
            System.Console.WriteLine();
            System.Console.WriteLine("Dostupne lode:");
            PrintAvailableShips();
            System.Console.WriteLine();
            System.Console.WriteLine($"Priklad pro svislou obrovskou lod v bunce a1: a1 {ShipCoordinateParser.CHAR_VERTICAL_DIRECTION}" +
                $" {ShipCoordinateParser.CHAR_SHIP_LARGE}");

            System.Console.WriteLine();
            _filler.Draw();
            System.Console.WriteLine();
            System.Console.WriteLine("Pokud chcete vycistit plochu, zadejte \'smaz\'.");
            System.Console.WriteLine("Pokud chcete automaticky naplnit pole, zadejte \'auto\'");
            System.Console.WriteLine("Pokud se chcete vratit do hlavniho menu, zadejte \'konc\'.");
            if (_filler.AvailableShips.Any())
                return;

            System.Console.WriteLine();
            System.Console.WriteLine("Pokracujte stiskem cokoliv jineho.");
        }

        /// <summary>
        /// Vypise zbyvajici pripustne lode.
        /// </summary>
        private void PrintAvailableShips()
        {
            System.Console.Write("   ");

            foreach (var pair in _filler.AvailableShips)
            {
                switch (pair.Key)
                {
                    case ShipSize.Tiny:
                        System.Console.Write($"Mala ({ShipCoordinateParser.CHAR_SHIP_TINY}): {pair.Value};  ");
                        break;
                    case ShipSize.Small:
                        System.Console.Write($"Stredni ({ShipCoordinateParser.CHAR_SHIP_SMALL}): {pair.Value};  ");
                        break;
                    case ShipSize.Medium:
                        System.Console.Write($"Velka ({ShipCoordinateParser.CHAR_SHIP_MEDIUM}): {pair.Value};  ");
                        break;
                    case ShipSize.Large:
                        System.Console.Write($"Obrovska ({ShipCoordinateParser.CHAR_SHIP_LARGE}): {pair.Value};  ");
                        break;
                }
            }

            System.Console.WriteLine();
        }

        #endregion

        #region Input handlers

        /// <summary>
        /// Metoda pro umisteni lodi na hracim poli na zaklade vstupu.
        /// </summary>
        /// <param name="input">Vstup od uzivatele.</param>
        private void PutShip(string input)
        {
            if (ShipCoordinateParser.TryParse(input, _filler.Size, out var x, out var y,
                    out var direction, out var shipSize))
            {
                if (!_filler.PutShip(x, y, shipSize, direction))
                    StateMsg = MSG_BAD_INPUT;
            }
            else
            {
                StateMsg = MSG_BAD_INPUT;
            }
        }

        /// <summary>
        /// Metoda pro vycisteni herniho pole.
        /// </summary>
        private void ClearField()
        {
            _filler = FieldFillerFactory.Create(_fieldSetup.Size);

            RemoveToken?.Invoke(_continueToken);
        }

        /// <summary>
        /// Metoda pro dokonceni tvorby pole a prechod do hry.
        /// </summary>
        private void BuildField()
        {
            if (_filler.AvailableShips.Any())
                return;

            var battlefield = _filler.Build();

            SetState(new NewGameState(StateMachine, battlefield, _fieldSetup));
        }

        /// <summary>
        /// Automaticky naplni pole.
        /// </summary>
        private void AutoFill()
        {
            _filler = _fieldFactory.CreateFilledFiller(_fieldSetup);
        }

        /// <summary>
        /// Metoda pro navrat do hlavniho menu.
        /// </summary>
        private void Exit()
        {
            SetState(new MainMenuState(StateMachine));
        }

		#endregion

		#region Initialization

		/// <summary>
		/// Metoda pro inicializaci zpracovani vstupu pro tvorbu hraciho pole.
		/// </summary>
		/// <param name="inputHandler">Instance tridy InputHandler pro registraci tokenu.</param>
		protected override void InitInputHandler(InputHandler inputHandler)
        {
            inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
            inputHandler.Add(REGEX_CLEAR, (_) => { ClearField(); });
            inputHandler.Add(REGEX_AUTO, (_) => { AutoFill(); });
            inputHandler.Add(REGEX_SHIP, PutShip);

            AddToken = (token) => { inputHandler.Add(token); };
            RemoveToken = (token) => { inputHandler.Remove(token); };
        }

        #endregion
    }
}

using SeaBattles.Console.FieldFillers;
using SeaBattles.Console.Input;
using SeaBattles.Console.Misc;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
    /// <summary>
    /// Stav tvorby herního pole.
    /// </summary>
    internal class FieldCreatingState : UserInputState<Game>
    {
        // Regex pro validaci lodi
        private const string REGEX_SHIP = @"^[a-zA-Z]\s*\d{1,2}\s*[sv]\s*[lmst]$";
        private const string REGEX_CLEAR = "^smaz$";
        private const string REGEX_EXIT = "^konc$";
        private const string REGEX_CONTINUE = "^.*$";

        private readonly FieldSetup _fieldSetup;

        private readonly InputToken _continueToken;
        private Action<InputToken>? AddToken;
        private Action<InputToken>? RemoveToken;

        private FieldFiller _filler;

        /// <summary>
        /// Konstruktor pro stav tvorby pole.
        /// </summary>
        /// <param name="game">Hra.</param>
        /// <param name="fieldSetup">Nastavení pole.</param>
        public FieldCreatingState(Game game, FieldSetup fieldSetup)
            : base(game)
        {
            _fieldSetup = fieldSetup;

            _filler = FieldFillerFactory.Create(_fieldSetup.Size);

            _continueToken = new(REGEX_CONTINUE, (_) => { BuildField(); });
        }

        #region Drawing

        /// <summary>
        /// Metoda pro vykreslení stavu tvorby pole.
        /// </summary>
        protected override void Draw()
        {
            if (!_filler.AvailableShips.Any())
                AddToken?.Invoke(_continueToken);

            System.Console.Clear();
            System.Console.WriteLine($"Postupně zadávejte pozice levého" +
                $" horního rohu vaší lodi, její směr " +
                $"a rozměr:");
            System.Console.WriteLine();
            System.Console.WriteLine("Dostupné směry:");
            System.Console.WriteLine($"   Svislá ({ShipCoordinateParser.CHAR_VERTICAL_DIRECTION})");
            System.Console.WriteLine($"   Vodorovná ({ShipCoordinateParser.CHAR_HORIZONTAL_DIRECTION})");
            System.Console.WriteLine();
            System.Console.WriteLine("Dostupné lodě:");
            PrintAvailableShips();
            System.Console.WriteLine();
            System.Console.WriteLine($"Příklad: a1 {ShipCoordinateParser.CHAR_VERTICAL_DIRECTION}" +
                $" {ShipCoordinateParser.CHAR_SHIP_LARGE}");

            System.Console.WriteLine();
            _filler.Draw();
            System.Console.WriteLine();
            System.Console.WriteLine("Pokud chcete vyčistit plochu, zadejte \'smaz\'.");
            System.Console.WriteLine();
            System.Console.WriteLine("Pokud se chcete vrátit do hlavního menu, zadejte \'konc\'.");
            if (_filler.AvailableShips.Any())
                return;

            System.Console.WriteLine();
            System.Console.WriteLine("Pokračujte stiskem cokoliv jiného.");
        }

        private void PrintAvailableShips()
        {
            System.Console.Write("   ");

            foreach (var pair in _filler.AvailableShips)
            {
                switch (pair.Key)
                {
                    case ShipSize.Tiny:
                        System.Console.Write($"Malý ({ShipCoordinateParser.CHAR_SHIP_TINY}): {pair.Value};  ");
                        break;
                    case ShipSize.Small:
                        System.Console.Write($"Střední ({ShipCoordinateParser.CHAR_SHIP_SMALL}): {pair.Value};  ");
                        break;
                    case ShipSize.Medium:
                        System.Console.Write($"Velký ({ShipCoordinateParser.CHAR_SHIP_MEDIUM}): {pair.Value};  ");
                        break;
                    case ShipSize.Large:
                        System.Console.Write($"Obrovský ({ShipCoordinateParser.CHAR_SHIP_LARGE}): {pair.Value};  ");
                        break;
                }
            }

            System.Console.WriteLine();
        }

        #endregion

        #region Input handlers

        /// <summary>
        /// Metoda pro umístění lodi na herní pole na základě vstupu.
        /// </summary>
        /// <param name="input">Vstup od uživatele.</param>
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
        /// Metoda pro vyčištění herního pole.
        /// </summary>
        private void ClearField()
        {
            _filler = FieldFillerFactory.Create(_fieldSetup.Size);

            RemoveToken?.Invoke(_continueToken);
        }

        /// <summary>
        /// Metoda pro dokončení tvorby pole a přechod do hry.
        /// </summary>
        private void BuildField()
        {
            if (_filler.AvailableShips.Any())
                return;

            var battlefield = _filler.Build();

            SetState(new NewGameState(StateMachine, battlefield, _fieldSetup));
        }

        /// <summary>
        /// Metoda pro návrat do hlavního menu.
        /// </summary>
        private void Exit()
        {
            SetState(new MainMenuState(StateMachine));
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Inicializace obslužného handleru pro vstup.
        /// </summary>
        protected override void InitInputHandler(InputHandler inputHandler)
        {
            inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
            inputHandler.Add(REGEX_CLEAR, (_) => { ClearField(); });
            inputHandler.Add(REGEX_SHIP, PutShip);

            AddToken = (token) => { inputHandler.Add(token); };
            RemoveToken = (token) => { inputHandler.Remove(token); };
        }

        #endregion
    }
}

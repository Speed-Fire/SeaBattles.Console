using SeaBattles.Console.Input;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
    /// <summary>
    /// Trida reprezentujici stav nastaveni obtiznosti hrace.
    /// </summary>
    internal class FieldSetupDifficultyState : UserInputState<Game>
    {
        private const string REGEX_AI = @"^\d*$";
        private const string REGEX_EXIT = @"^konc$";

        private readonly FieldSetup _setup;

        /// <summary>
        /// Konstruktor tridy FieldSetupDifficultyState.
        /// </summary>
        /// <param name="game">Instance hry.</param>
        /// <param name="setup">Nastaveni pole.</param>
        public FieldSetupDifficultyState(Game game, FieldSetup setup)
            : base(game)
        {
            _setup = setup;
        }

        #region Input handlers

        /// <summary>
        /// Pokusi zparsit obtiznost UI, a pokud to bylo uspesne,
        ///  prevede automat na novy stav.
        /// </summary>
        /// <param name="input"></param>
        private void SetAI(string input)
        {
            // parsovani a kontrola vstupu.
            var val = int.Parse(input);

            if (val < 1 || val > 3)
            {
                StateMsg = MSG_BAD_INPUT;

                return;
            }

            _setup.Difficulty = val switch
            {
                1 => Difficulty.Easy,
                2 => Difficulty.Normal,
                3 => Difficulty.Hard,
                _ => Difficulty.Hard
            };

            // prechod na stav tvorby hraciho pole.
            SetState(new FieldCreatingState(StateMachine, _setup));
        }

        /// <summary>
        /// Vrati do hlavniho menu.
        /// </summary>
        private void Exit()
        {
            SetState(new MainMenuState(StateMachine));
        }

        #endregion

        #region Drawing

        /// <summary>
        /// Metoda pro vykresleni uzivatelskeho rozhrani pro nastaveni obtiznosti hrace.
        /// </summary>
        protected override void Draw()
        {
            System.Console.Clear();

            System.Console.WriteLine("Zvolte si rozum Umele Inteligence:");

            var color = System.Console.ForegroundColor;

            System.Console.Write("1. ");

            System.Console.ForegroundColor = ConsoleColor.DarkGreen;
            System.Console.WriteLine("Hloupy");
            System.Console.ForegroundColor = color;

            System.Console.Write("2. ");

            System.Console.ForegroundColor = ConsoleColor.DarkYellow;
            System.Console.WriteLine("Chytry");
            System.Console.ForegroundColor = color;

            System.Console.Write("3. ");

            System.Console.ForegroundColor = ConsoleColor.DarkRed;
            System.Console.WriteLine("Moudry");
            System.Console.ForegroundColor = color;


            System.Console.WriteLine();
            System.Console.WriteLine("Pokud se chcete vratit do hlavniho menu, zadejte 'konc'.");
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Metoda pro inicializaci zpracovani vstupu pro nastaveni obtiznosti hrace.
        /// </summary>
        /// <param name="inputHandler">Instance tridy InputHandler pro registraci tokenu.</param>
        protected override void InitInputHandler(InputHandler inputHandler)
        {
            inputHandler.Add(REGEX_AI, SetAI);
            inputHandler.Add(REGEX_EXIT, (_) => { Exit(); });
        }

        #endregion
    }

}

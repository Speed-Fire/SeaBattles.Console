using SeaBattles.Console.Input;

namespace SeaBattles.Console.States.Menus
{
    /// <summary>
    /// Třída reprezentující hlavní menu ve hře.
    /// </summary>
    internal class MainMenuState : UserInputState<Game>
    {
        public MainMenuState(Game game) : base(game)
        {

        }

        #region Drawing

        /// <summary>
        /// Metoda pro vykreslení hlavního menu.
        /// </summary>
        protected override void Draw()
        {
            System.Console.Clear();

            System.Console.WriteLine("    Vitam Vas ve hre Lode!");
            System.Console.WriteLine("Zvolte prosim cislo pro pokracovani:");
            System.Console.WriteLine("1. Nova hra");
            System.Console.WriteLine("2. Nahrat hru");
            System.Console.WriteLine("3. Staci");
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Metoda pro inicializaci zpracování vstupu pro hlavní menu.
        /// </summary>
        /// <param name="inputHandler">Instance třídy InputHandler pro registraci tokenů.</param>
        protected override void InitInputHandler(InputHandler inputHandler)
        {
            inputHandler.Add(@"^1$", (_) => { SetState(new FieldSetupSizeState(StateMachine)); });
            inputHandler.Add(@"^2$", (_) => { SetState(new LoadGameState(StateMachine)); });
            inputHandler.Add(@"^3$", (_) => { SetState(null); });
        }

        #endregion
    }
}

using SeaBattles.Console.Input;

namespace SeaBattles.Console.States.Engine
{
	/// <summary>
	/// Trida reprezentujici stav chyby pri ulozeni hry.
	/// </summary>
	internal class SavingErrorState : UserInputState<Console.Engine>
    {
        public SavingErrorState(Console.Engine engine)
            : base(engine)
        {

        }

		#region Drawing

		/// <summary>
		/// Metoda pro vykresleni stavu chyby pri ulozeni hry.
		/// </summary>
		protected override void Draw()
        {
            System.Console.Clear();

            System.Console.WriteLine();
            System.Console.WriteLine("Bohuzel nepodarilo se ulozit hru.");

            System.Console.WriteLine();
            System.Console.WriteLine("Pokracujte stiskem libovolne klavesy...");

            System.Console.WriteLine();
        }

		#endregion

		#region Initialization

		/// <summary>
		/// Metoda pro inicializaci zpracovani vstupu pro stav chyby pri ulozeni hry.
		/// </summary>
		/// <param name="inputHandler">Instance tridy InputHandler pro registraci tokenu.</param>
		protected override void InitInputHandler(InputHandler inputHandler)
        {
            inputHandler.Add(@"^.*$", (_) => { SetState(new PlayerMoveState(StateMachine)); });
        }

		#endregion
	}
}

using SeaBattles.Console.Input;

namespace SeaBattles.Console.States.Engine
{
	/// <summary>
	/// Trida reprezentujici stav uspesne ulozene hry.
	/// </summary>
    internal class SavingSuccessState : UserInputState<Console.Engine>
    {
        public SavingSuccessState(Console.Engine engine)
            : base(engine)
        {
            
        }

		#region Drawing

		/// <summary>
		/// Metoda pro vykresleni stavu uspesne ulozeni hry.
		/// </summary>
		protected override void Draw()
		{
			System.Console.Clear();

			System.Console.WriteLine();
			System.Console.WriteLine("Hra byla uspesne ulozena.");

			System.Console.WriteLine();
			System.Console.WriteLine("Pokracujte stiskem libovolne klavesy...");
		}

		#endregion

		#region Initialization

		/// <summary>
		/// Metoda pro inicializaci zpracovani vstupu pro stav uspesne ulozeni hry.
		/// </summary>
		/// <param name="inputHandler">Instance tridy InputHandler pro registraci tokenu.</param>
		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add("^.*$", (_) => { SetState(null); });
		}

		#endregion
	}
}

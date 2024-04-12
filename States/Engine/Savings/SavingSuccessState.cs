using SeaBattles.Console.Input;

namespace SeaBattles.Console.States.Engine
{
    internal class SavingSuccessState : UserInputState<Console.Engine>
    {
        public SavingSuccessState(Console.Engine engine)
            : base(engine)
        {
            
        }

		#region Drawing

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

		protected override void InitInputHandler(InputHandler inputHandler)
		{
			inputHandler.Add("^.*$", (_) => { SetState(null); });
		}

		#endregion
	}
}

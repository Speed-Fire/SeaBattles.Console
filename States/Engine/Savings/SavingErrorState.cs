using SeaBattles.Console.Input;

namespace SeaBattles.Console.States.Engine
{
    internal class SavingErrorState : UserInputState<Console.Engine>
    {
        public SavingErrorState(Console.Engine engine)
            : base(engine)
        {

        }

		#region Drawing

		protected override void Draw()
        {
            System.Console.Clear();

            System.Console.WriteLine();
            System.Console.WriteLine("Bohuzel nepodarilo se ulozit hru.");

            System.Console.WriteLine();
            System.Console.WriteLine("Chcete se vratit ke hre? (a/n)");

            System.Console.WriteLine();
        }

		#endregion

		#region Initialization

		protected override void InitInputHandler(InputHandler inputHandler)
        {
            inputHandler.Add(@"^a$", (_) => { SetState(new PlayerMoveState(StateMachine)); });
            inputHandler.Add(@"^n$", (_) => { SetState(null); });
        }

		#endregion
	}
}

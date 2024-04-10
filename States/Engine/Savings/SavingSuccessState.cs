namespace SeaBattles.Console.States.Engine
{
    internal class SavingSuccessState : IState
    {
        private readonly Console.Engine _engine;

        public SavingSuccessState(Console.Engine engine)
        {
            _engine = engine;
        }

        public void Invoke()
        {
            System.Console.Clear();

            System.Console.WriteLine();
            System.Console.WriteLine("Hra byla uspesne ulozena.");

            System.Console.WriteLine();
            System.Console.WriteLine("Pokracujte stickem libovolne klavesy...");

            System.Console.ReadLine();

            _engine.SetState(null);
        }
    }
}

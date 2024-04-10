using SeaBattles.Console.Input;

namespace SeaBattles.Console.States.Engine
{
    internal class SavingErrorState : IState
    {
        private readonly Console.Engine _engine;
        private readonly InputHandler _inputHandler;

        public SavingErrorState(Console.Engine engine)
        {
            _engine = engine;
            _inputHandler = new();

            InitInputHandler();
        }

        public void Invoke()
        {
            Draw();

            var input = (System.Console.ReadLine() ?? string.Empty).Trim();

            if (!_inputHandler.Handle(input))
            {
                _engine.StateMsg = Console.Engine.MSG_BAD_INPUT;
            }
        }

        private void Draw()
        {
            System.Console.Clear();

            System.Console.WriteLine();
            System.Console.WriteLine("Bohuzel nepodarilo se ulozit hru.");

            System.Console.WriteLine();
            System.Console.WriteLine("Chcete se vratit ke hre? (a/n)");

            System.Console.WriteLine();
            System.Console.WriteLine();

            System.Console.WriteLine(_engine.StateMsg.PadLeft(15));
            System.Console.WriteLine();
        }

        private void InitInputHandler()
        {
            _inputHandler.Add(@"^a$", (_) => { _engine.SetState(new PlayerMoveState(_engine)); });
            _inputHandler.Add(@"^n$", (_) => { _engine.SetState(null); });
        }
    }
}

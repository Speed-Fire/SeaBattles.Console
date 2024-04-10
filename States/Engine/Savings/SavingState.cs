using System;
using SeaBattles.Console.Level;

namespace SeaBattles.Console.States.Engine
{
    internal class SavingState : IState
    {
        private readonly Console.Engine _engine;

        public SavingState(Console.Engine engine)
        {
            _engine = engine;
        }

        public void Invoke()
        {
            var res = LevelSerializer.Serialize(_engine.LevelData);

            _engine.SetState(res ? new SavingSuccessState(_engine) : new SavingErrorState(_engine));
        }
    }
}

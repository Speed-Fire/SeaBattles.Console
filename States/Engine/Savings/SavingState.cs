using SeaBattles.Console.Level;

namespace SeaBattles.Console.States.Engine
{
    internal class SavingState : StateBase<Console.Engine>
    {
        public SavingState(Console.Engine engine)
            : base(engine)
        {
            
        }

        public override void Invoke()
        {
            var res = LevelSaver.Save(StateMachine.LevelData);

            SetState(res ? new SavingSuccessState(StateMachine) : new SavingErrorState(StateMachine));
        }
    }
}

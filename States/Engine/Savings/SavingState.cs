using SeaBattles.Console.Level;

namespace SeaBattles.Console.States.Engine
{
    /// <summary>
    /// Trida reprezentujici stav ulozeni hry.
    /// </summary>
    internal class SavingState : StateBase<Console.Engine>
    {
        public SavingState(Console.Engine engine)
            : base(engine)
        {
            
        }

        /// <summary>
        /// Metoda pro spusteni tohohle stavu.
        /// </summary>
        public override void Invoke()
        {
            // pokusi ulozit hru a vrati vysledek pokusu.
            var res = LevelSaver.Save(StateMachine.LevelData);

            // pokud pokus neni priznivy, prevede automat na stav chyby pru ulozeni;
            //  jinak na stav uspesneho ulozeni.
            SetState(res ? new SavingSuccessState(StateMachine) : new SavingErrorState(StateMachine));
        }
    }
}

using SeaBattles.Console.States;

namespace SeaBattles.Console
{
    /// <summary>
    /// Abstraktni trida reprezentujici stavovy automat v aplikaci.
    /// </summary>
    internal abstract class StateMachine
    {
        /// <summary>
        /// Aktualni stav stavoveho automatu.
        /// </summary>
        protected IState? CurrentState { get; set; }

        /// <summary>
        /// Spusti stavovy automat.
        /// </summary>
        public void Start()
        {
            while (CurrentState is not null)
            {
                CurrentState.Invoke();
            }
        }

        /// <summary>
        /// Nastavi novy stav stavoveho automatu.
        /// </summary>
        /// <param name="state">Novy stav, ktery ma byt nastaven.</param>
        public void SetState(IState? state)
        {
            CurrentState = state;
        }
    }

}

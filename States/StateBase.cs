namespace SeaBattles.Console.States
{
    /// <summary>
    /// Abstraktni trida pro zakladni stav v aplikaci.
    /// </summary>
    /// <typeparam name="TStateMachine">Typ stavoveho automatu.</typeparam>
    internal abstract class StateBase<TStateMachine> : IState
        where TStateMachine : StateMachine
    {
        /// <summary>
        /// Stavovy automat, ktery tento stav pouziva.
        /// </summary>
        protected TStateMachine StateMachine { get; }

        /// <summary>
        /// Konstruktor tridy StateBase.
        /// </summary>
        /// <param name="stateMachine">Stavovy automat, ktery tento stav pouziva.</param>
        protected StateBase(TStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        /// <summary>
        /// Metoda pro spusteni tohoto stavu.
        /// </summary>
        public abstract void Invoke();

        /// <summary>
        /// Metoda pro nastaveni noveho stavu v ramci stavoveho automatu.
        /// </summary>
        /// <param name="state">Novy stav, ktery ma byt nastaven.</param>
        protected void SetState(StateBase<TStateMachine>? state)
        {
            StateMachine.SetState(state);
        }
    }

}

namespace SeaBattles.Console.States
{
    /// <summary>
    /// Abstraktní třída pro základní stav v aplikaci.
    /// </summary>
    /// <typeparam name="TStateMachine">Typ stavového automatu.</typeparam>
    internal abstract class StateBase<TStateMachine> : IState
        where TStateMachine : StateMachine
    {
        /// <summary>
        /// Stavový automat, který tento stav používá.
        /// </summary>
        protected TStateMachine StateMachine { get; }

        /// <summary>
        /// Konstruktor třídy StateBase.
        /// </summary>
        /// <param name="stateMachine">Stavový automat, který tento stav používá.</param>
        protected StateBase(TStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }

        /// <summary>
        /// Metoda pro spuštění tohoto stavu.
        /// </summary>
        public abstract void Invoke();

        /// <summary>
        /// Metoda pro nastavení nového stavu v rámci stavového automatu.
        /// </summary>
        /// <param name="state">Nový stav, který má být nastaven.</param>
        protected void SetState(StateBase<TStateMachine>? state)
        {
            StateMachine.SetState(state);
        }
    }

}

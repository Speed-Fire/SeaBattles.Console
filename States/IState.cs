namespace SeaBattles.Console.States
{
    /// <summary>
    /// Rozhrani pro stav v aplikaci.
    /// </summary>
    internal interface IState
    {
        /// <summary>
        /// Metoda pro spusteni stavu.
        /// </summary>
        void Invoke();
    }
}

using SeaBattles.Console.Input;

namespace SeaBattles.Console.States
{
    /// <summary>
    /// Abstraktni trida reprezentujici stav uzivatelskeho vstupu v aplikaci.
    /// </summary>
    /// <typeparam name="TStateMachine">Typ stavoveho automatu.</typeparam>
    internal abstract class UserInputState<TStateMachine> : StateBase<TStateMachine>
        where TStateMachine : StateMachine
    {
        protected const string MSG_BAD_INPUT = "spatny vstup";

        private readonly InputHandler _inputHandler;

        private string _stateMsg = string.Empty;
        protected string StateMsg
        {
            private get
            {
                var tmp = _stateMsg;

                // automaticke mazani stavajici zpravy o chybe pri jeji ziskani.
                _stateMsg = string.Empty;

                return tmp;
            }

            set
            {
                _stateMsg = value;
            }
        }

        /// <summary>
        /// Konstruktor abstraktni tridy UserInputState.
        /// </summary>
        /// <param name="stateMachine">Stavovy automat, ktery tento stav pouziva.</param>
        public UserInputState(TStateMachine stateMachine) : base(stateMachine)
        {
            _inputHandler = new();

            StateMsg = string.Empty;

            InitInputHandler(_inputHandler);
        }

        /// <summary>
        /// Metoda pro spusteni tohoto stavu.
        /// </summary>
        public override void Invoke()
        {
            DrawInternal();

            var input = (System.Console.ReadLine() ?? string.Empty).Trim();

            if (!_inputHandler.Handle(input))
            {
                StateMsg = MSG_BAD_INPUT;
            }
        }

        /// <summary>
        /// Metoda pro vykresleni obecne informaci a informaci spojene s konkretni realizaci stavu.
        /// </summary>
        private void DrawInternal()
        {
            Draw();

            var color = System.Console.ForegroundColor;
            System.Console.ForegroundColor = ConsoleColor.DarkRed;

            System.Console.WriteLine();
            System.Console.WriteLine(StateMsg.PadLeft(15));
            System.Console.WriteLine();

            System.Console.ForegroundColor = color;
        }

        /// <summary>
        /// Metoda pro vykresleni informaci spojenych s timto stavem.
        /// </summary>
        protected abstract void Draw();

        /// <summary>
        /// Metoda pro inicializaci zpracovani vstupu.
        /// </summary>
        /// <param name="inputHandler">Instance tridy InputHandler pro registraci tokenu.</param>
        protected abstract void InitInputHandler(InputHandler inputHandler);
    }

}

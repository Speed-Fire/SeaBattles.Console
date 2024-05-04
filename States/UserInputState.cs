using SeaBattles.Console.Input;

namespace SeaBattles.Console.States
{
    /// <summary>
    /// Abstraktní třída reprezentující stav uživatelského vstupu v aplikaci.
    /// </summary>
    /// <typeparam name="TStateMachine">Typ stavového automatu.</typeparam>
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

                _stateMsg = string.Empty;

                return tmp;
            }

            set
            {
                _stateMsg = value;
            }
        }

        /// <summary>
        /// Konstruktor abstraktní třídy UserInputState.
        /// </summary>
        /// <param name="stateMachine">Stavový automat, který tento stav používá.</param>
        public UserInputState(TStateMachine stateMachine) : base(stateMachine)
        {
            _inputHandler = new();

            StateMsg = string.Empty;

            InitInputHandler(_inputHandler);
        }

        /// <summary>
        /// Metoda pro spuštění tohoto stavu.
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
        /// Metoda pro vykreslení informací spojených s tímto stavem.
        /// </summary>
        protected abstract void Draw();

        /// <summary>
        /// Metoda pro inicializaci zpracování vstupu.
        /// </summary>
        /// <param name="inputHandler">Instance třídy InputHandler pro registraci tokenů.</param>
        protected abstract void InitInputHandler(InputHandler inputHandler);
    }

}

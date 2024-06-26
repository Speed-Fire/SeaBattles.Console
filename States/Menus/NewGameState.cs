﻿using SeaBattles.Console.AI;
using SeaBattles.Console.FieldFactories;
using SeaBattles.Console.Models;

namespace SeaBattles.Console.States.Menus
{
    /// <summary>
    /// Trida reprezentujici stav nove hry.
    /// </summary>
    internal class NewGameState : StateBase<Game>
    {
        private const int MIN_HINT_COUNT = 3;

        private readonly BattleField _userField;
        private readonly FieldSetup _fieldSetup;

        // tovarna pro tvorbu pocitacoveho hraciho pole.
        private readonly IFieldFactory _fieldFactory;

        /// <summary>
        /// Konstruktor tridy NewGameState.
        /// </summary>
        /// <param name="game">Instance hry.</param>
        /// <param name="userField">Hraci pole hrace.</param>
        /// <param name="fieldSetup">Nastaveni pole.</param>
        public NewGameState(Game game, BattleField userField, FieldSetup fieldSetup)
            : base(game)
        {
            _userField = userField;
            _fieldSetup = fieldSetup;

            _fieldFactory = new ComputerFieldFactory();
        }

        /// <summary>
        /// Spusti novou hru.
        /// </summary>
        public override void Invoke()
        {
            var compField = _fieldFactory.CreateBattlefield(_fieldSetup);

            var levelData = new LevelData(GetAI(_userField), _userField, compField,
                MIN_HINT_COUNT + (_userField.Size - 7));

            var engine = new Console.Engine(levelData);

            SetState(new PlayingState(StateMachine, engine));
        }

        /// <summary>
        /// Ziska umelou inteligenci pro hru na zaklade obtiznosti.
        /// </summary>
        /// <param name="userField">Hraci pole hrace.</param>
        /// <returns>Instance tridy AIPlayer odpovidajici obtiznosti.</returns>
        private AIPlayer GetAI(BattleField userField)
        {
            return _fieldSetup.Difficulty switch
            {
                Difficulty.Normal => new AIPlayerNormal(userField),
                Difficulty.Hard => new AIPlayerHard(userField),
                _ => new AIPlayerEasy(userField),
            };
        }
    }
}

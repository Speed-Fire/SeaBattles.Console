using SeaBattles.Console.Models;
using SeaBattles.Console.States.Engine;

namespace SeaBattles.Console
{
    /// <summary>
    /// Trida predstavujici herni engine.
    /// </summary>
    internal class Engine : StateMachine
    {
        public const string MSG_SHIP_HIT = "porazeni";
        public const string MSG_SHIP_DESTROYED = "znicena";
        public const string MSG_FIRE_MISSED = "minuti cile";

        /// <summary>
        /// Data pro aktualni uroven.
        /// </summary>
        public LevelData LevelData { get; }

        /// <summary>
        /// Konstruktor tridy Engine.
        /// </summary>
        /// <param name="levelData">Data pro aktualni uroven.</param>
        public Engine(LevelData levelData)
        {
            LevelData = levelData;

            SetState(new PlayerMoveState(this));
        }
    }
}

using SeaBattles.Console.Models;

namespace SeaBattles.Console.FieldFactories
{
    internal interface IFieldFactory
    {
        BattleField CreateBattlefield(FieldSetup setup);
    }
}

using SeaBattles.Console.FieldFactories;

namespace SeaBattles.Console.GameInitializers
{
	internal class NewGameEngineFactory : IEngineFactory
	{
		public Engine CreateEngine()
		{
			var setup = FieldSetuper.GetSetup();

			var usFieldFactory = new UserFieldFactory();
			var compFieldFactory = new ComputerFieldFactory();

			var userField = usFieldFactory.CreateBattlefield(setup);
			var compField = compFieldFactory.CreateBattlefield(setup);

			return new(userField, compField);
		}
	}
}

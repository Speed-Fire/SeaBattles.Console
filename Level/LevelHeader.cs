using SeaBattles.Console.Models;

namespace SeaBattles.Console.Level
{
	internal record LevelHeader(DateTime DateTime, Difficult Difficulty, int FieldSize, string Path);
}

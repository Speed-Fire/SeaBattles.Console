using SeaBattles.Console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SeaBattles.Console.Level
{
	internal class LevelSerializer
	{
		private static readonly string _directory =
			Path.Combine(Directory.GetCurrentDirectory(), "Saves");

		public static bool Serialize(LevelData levelData)
		{
			try
			{
				var dtstr = DateTime.Now.ToString("yyyy.MM.dd HH.mm.ss")
					.Replace(" ", string.Empty)
					.Replace(".", string.Empty);

				var filename = Path.Combine(_directory, dtstr);

				var serializer = new DataContractSerializer(typeof(LevelData));

				using var filestream = File.OpenWrite(filename);

				using var xmlWriter = XmlDictionaryWriter.CreateTextWriter(filestream);

				serializer.WriteObject(xmlWriter, levelData);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public static LevelData? Deserialize(string path)
		{
			try
			{
				var serializer = new DataContractSerializer(typeof(LevelData));

				using var filestream = File.OpenRead(path);

				using var xmlReader = XmlDictionaryReader.CreateTextReader(filestream, XmlDictionaryReaderQuotas.Max);

				var levelData = (LevelData?)serializer.ReadObject(xmlReader);

				return levelData;
			}
			catch
			{
				return null;
			}
		}
	}
}

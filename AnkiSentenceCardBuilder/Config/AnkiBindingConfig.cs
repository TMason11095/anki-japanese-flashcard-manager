using AnkiJapaneseFlashcardManager.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.Config
{
	public static class AnkiBindingConfig
	{
		public static readonly AnkiBindingTagsDTO Bindings;

		static AnkiBindingConfig()
		{
			//Get the bindings JSON data
			string jsonFilePath = "ankiBindingTags.json";
			string json = File.ReadAllText(jsonFilePath);
			//Setup the settings to ignore any extra properties
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				MissingMemberHandling = MissingMemberHandling.Ignore
			};
			//Set the bindings
			Bindings = JsonConvert.DeserializeObject<AnkiBindingTagsDTO>(json, settings);
		}
	}
}

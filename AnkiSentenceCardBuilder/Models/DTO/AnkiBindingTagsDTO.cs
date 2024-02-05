using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.Models.DTO
{
	public class AnkiBindingTagsDTO
	{
		[JsonProperty("deckTag")]
		public string DeckTag { get; }
		[JsonProperty("resourceDecks")]
		public ResourceDecksDTO ResourceDecks { get; }
		[JsonProperty("newDecks")]
		public NewDecksDTO NewDecks { get; }
	}

	public class ResourceDecksDTO
	{
		[JsonProperty("kanji")]
		public string Kanji { get; }
	}

	public class NewDecksDTO
	{
		[JsonProperty("kanji")]
		public string Kanji { get; }
	}
}

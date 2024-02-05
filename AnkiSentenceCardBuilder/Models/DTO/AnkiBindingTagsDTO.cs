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
		public string DeckTag { get; protected set; }
		[JsonProperty("resourceDecks")]
		public ResourceDecksDTO ResourceDecks { get; protected set; }
		[JsonProperty("newDecks")]
		public NewDecksDTO NewDecks { get; protected set; }
	}

	public class ResourceDecksDTO
	{
		[JsonProperty("kanji")]
		public string Kanji { get; protected set; }
	}

	public class NewDecksDTO
	{
		[JsonProperty("kanji")]
		public string Kanji { get; protected set; }
	}
}

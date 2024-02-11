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
		[JsonProperty("learningDecks")]
		public LearningDecksDTO LearningDecks { get; protected set; }
		[JsonProperty("noteTags")]
		public NoteTagsDTO NoteTags { get; protected set; }
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

	public class LearningDecksDTO
	{
		[JsonProperty("kanji")]
		public string Kanji { get; protected set; }
	}

	public class NoteTagsDTO
	{
		[JsonProperty("kanjiId")]
		public string KanjiId { get; protected set; }
		[JsonProperty("subKanjiId")]
		public string SubKanjiId { get; protected set; }
	}
}

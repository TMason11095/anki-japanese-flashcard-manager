using AnkiJapaneseFlashcardManager.Config;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services
{
	public class KanjiNoteService
	{
		private readonly Anki2Controller _anki2Controller;

		public KanjiNoteService(Anki2Controller anki2Controller)
		{
			_anki2Controller = anki2Controller;
		}

		public List<Note> GetKanjiNotes(List<Note> deckNotes)//Note
		{
			//Get the kanji note tag name
			string kanjiTagName = AnkiBindingConfig.Bindings.NoteTags.KanjiId;
			//Return the tagged notes
			return _anki2Controller.GetTaggedNotes(deckNotes, kanjiTagName);
		}
	}
}

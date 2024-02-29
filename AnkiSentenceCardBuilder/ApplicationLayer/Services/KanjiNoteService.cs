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

		public List<Note> PullAllSubKanjiNotesFromNoteList(ref List<Note> noteList, List<Note> originalKanjiNotes)//Note
		{
			//Return empty list if either input list is empty
			if (originalKanjiNotes.Count == 0 || noteList.Count == 0) { return new List<Note>(); }
			//Get sub kanji ids from the input original kanji notes
			List<string> subKanjiIds = _anki2Controller.GetSubKanjiIds(originalKanjiNotes);
			//Get kanji notes from the input note list with matching kanji ids
			List<Note> subKanjiNotes = _anki2Controller.GetNotesByKanjiIds(noteList, subKanjiIds);
			//Remove found notes from the input note list
			noteList = noteList.Except(subKanjiNotes).ToList();
			//Recursively call to grab any additional sub kanji notes based on the currently pulled kanji notes
			subKanjiNotes.AddRange(PullAllSubKanjiNotesFromNoteList(ref noteList, subKanjiNotes));
			//Return the full list of all related sub kanji notes
			return subKanjiNotes;
		}
	}
}

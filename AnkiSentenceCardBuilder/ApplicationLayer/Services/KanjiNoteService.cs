using AnkiJapaneseFlashcardManager.ApplicationLayer.Config;
using AnkiJapaneseFlashcardManager.ApplicationLayer.Helpers;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services
{
    public class KanjiNoteService
	{
		public KanjiNoteService()
		{
		}

		public IEnumerable<Note> GetKanjiNotes(IEnumerable<Note> deckNotes)//Note
		{
			//Get the kanji note tag name
			string kanjiTagName = AnkiBindingConfig.Bindings.NoteTags.KanjiId;
			//Return the tagged notes
			return GetTaggedNotes(deckNotes, kanjiTagName);
		}

		public IEnumerable<string> GetSubKanjiIds(IEnumerable<Note> kanjiNotes)//Note
		{
			//Get sub kanji id tag
			string subKanjiIdTag = AnkiBindingConfig.Bindings.NoteTags.SubKanjiId;
			//Get all the note tags
			IEnumerable<string> allTags = kanjiNotes.SelectMany(n => n.TagsList);
			//Return the sub kanji ids
			return NoteHelper.GetIdsFromTagList(allTags, subKanjiIdTag)
					.Distinct();//Filter out duplicate entries (Multiple kanji can share the same sub kanji id)
		}

		public List<Note> PullAllSubKanjiNotesFromNoteList(ref List<Note> noteList, List<Note> originalKanjiNotes)//Note
		{
			//Return empty list if either input list is empty
			if (originalKanjiNotes.Count == 0 || noteList.Count == 0) { return new List<Note>(); }
			//Get sub kanji ids from the input original kanji notes
			IEnumerable<string> subKanjiIds = GetSubKanjiIds(originalKanjiNotes);
			//Get kanji notes from the input note list with matching kanji ids
			List<Note> subKanjiNotes = GetNotesByKanjiIds(noteList, subKanjiIds).ToList();
			//Remove found notes from the input note list
			noteList = noteList.Except(subKanjiNotes).ToList();
			//Recursively call to grab any additional sub kanji notes based on the currently pulled kanji notes
			subKanjiNotes.AddRange(PullAllSubKanjiNotesFromNoteList(ref noteList, subKanjiNotes));
			//Return the full list of all related sub kanji notes
			return subKanjiNotes;
		}

		public IEnumerable<Note> GetTaggedNotes(IEnumerable<Note> deckNotes, string noteTagName)//Note
		{
			//Filter to find the notes that use the specified tag
			return deckNotes.Where(n => n.TagsList.Exists(t => t.StartsWith(noteTagName)));
		}

		public IEnumerable<Note> GetNotesByKanjiIds(IEnumerable<Note> kanjiNotes, IEnumerable<string> kanjiIds)//Note
		{
			//Get kanji id tag
			string kanjiIdTag = AnkiBindingConfig.Bindings.NoteTags.KanjiId;
			//Return the kanji notes with matching ids
			return kanjiNotes.Where(n => NoteHelper.GetIdsFromTagList(n.TagsList, kanjiIdTag).ToList().Exists(id => kanjiIds.Contains(id)));
		}
	}
}

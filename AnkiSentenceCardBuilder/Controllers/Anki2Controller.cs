using AnkiJapaneseFlashcardManager.Config;
using AnkiSentenceCardBuilder.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiSentenceCardBuilder.Controllers
{
    public class Anki2Controller : IDisposable
	{
        private readonly Anki2Context _context;

        public Anki2Controller(Anki2Context context)
        {
            _context = context;
        }

        public Anki2Controller(string dbPath) : this(new Anki2Context(dbPath))
        {
        }

		public void Dispose()
		{
			SqliteConnection.ClearPool((SqliteConnection) _context.Database.GetDbConnection());
			_context.Dispose();
		}

		public List<T> GetTable<T>() where T : class
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        private static string DecodeBlob(byte[] blob)
        {
            return System.Text.Encoding.UTF8.GetString(blob);
		}

		public List<Deck> GetTaggedDecks(string deckTagName)
        {
			//Get the deck tag (prefix for the full tag)
			string deckTag = AnkiBindingConfig.Bindings.DeckTag;
			//Build the full tag
            string fullDeckTag = deckTag + deckTagName;
			//Get all the decks
			var decks = _context.Decks.Include(d => d.Cards);
			//Remap to decode the description field (Kind) (Convert to List as the following .Where() tries calling DecodeBlob() and fails if you don't)
			var deckDescs = decks.Select(d => new
			{
				deck = d,
				description = DecodeBlob(d.Kind)
			}).ToList();
			//Filter to find the decks with the tag in its description
			var taggedDecks = deckDescs
				.Where(d => d.description.Contains(fullDeckTag, StringComparison.OrdinalIgnoreCase))
				.Select(d => d.deck)
				.ToList();
			//Return the list
			return taggedDecks;
		}

		public List<Deck> GetResourceKanjiDecks()
        {
            //Get resource kanji deck tag name
            string deckTagName = AnkiBindingConfig.Bindings.ResourceDecks.Kanji;
			//Return the decks
			return GetTaggedDecks(deckTagName);
		}

		public List<Deck> GetNewKanjiDecks()
		{
			//Get new kanji deck tag name
			string deckTagName = AnkiBindingConfig.Bindings.NewDecks.Kanji;
			//Return the decks
			return GetTaggedDecks(deckTagName);
		}

		public List<Deck> GetLearningKanjiDecks()
		{
			//Get new kanji deck tag name
			string deckTagName = AnkiBindingConfig.Bindings.LearningDecks.Kanji;
			//Return the decks
			return GetTaggedDecks(deckTagName);
		}

		//public Deck GetDeckById(long deckId)//TODO
		//{
		//	return null;
		//}

		public List<Note> GetDeckNotes(long deckId)
		{
			//Return the notes from unique card entries with the given deck id
			return _context.Cards
					.Where(c => c.DeckId == deckId) //Grab cards with matching deck id
					.Select(c => c.Note) //Grab the notes
					.Distinct() //Filter out duplicate entries
					.ToList();
		}

		public List<Note> GetTaggedNotes(List<Note> deckNotes, string noteTagName)
		{
			//Filter to find the notes that use the specified tag
			return deckNotes.Where(n => n.TagsList.Exists(t => t.StartsWith(noteTagName))).ToList();
		}

		public List<Note> GetKanjiNotes(List<Note> deckNotes)
		{
			//Get the kanji note tag name
			string kanjiTagName = AnkiBindingConfig.Bindings.NoteTags.KanjiId;
			//Return the tagged notes
			return GetTaggedNotes(deckNotes, kanjiTagName);
		}

		//public Note GetNoteById(long noteId)//TODO
		//{
		//	return null;
		//}

		//public List<long> GetSubKanjiIds(Note kanjiNote)//TODO
		//{
		//	return null;
		//}

		public List<string> GetSubKanjiIds(List<Note> kanjiNotes)
		{
			//Get sub kanji id tag
			string subKanjiIdTag = AnkiBindingConfig.Bindings.NoteTags.SubKanjiId;
			//Get all the note tags
			List<string> allTags = kanjiNotes.SelectMany(n => n.TagsList).ToList();
			//Return the sub kanji ids
			return GetIdsFromTagList(allTags, subKanjiIdTag)
					.Distinct()//Filter out duplicate entries (Multiple kanji can share the same sub kanji id)
					.ToList();
		}

		public List<Note> GetNotesByKanjiIds(IEnumerable<Note> kanjiNotes, IEnumerable<string> kanjiIds)
		{
			//Get kanji id tag
			string kanjiIdTag = AnkiBindingConfig.Bindings.NoteTags.KanjiId;
			//Return the kanji notes with matching ids
			return kanjiNotes.Where(n => GetIdsFromTagList(n.TagsList, kanjiIdTag).Exists(id => kanjiIds.Contains(id))).ToList();
		}

		public List<string> GetIdsFromTagList(List<string> tagList, string tag)
		{
			return tagList.Where(t => t.StartsWith(tag))
						.Select(t => t.Substring(tag.Length))
						.ToList();
		}

		public List<Note> PullAllSubKanjiNotesFromNoteList(ref List<Note> noteList, List<Note> originalKanjiNotes)
		{
			//Return empty list if either input list is empty
			if (originalKanjiNotes.Count == 0 || noteList.Count == 0) { return new List<Note>(); }
			//Get sub kanji ids from the input original kanji notes
			List<string> subKanjiIds = GetSubKanjiIds(originalKanjiNotes);
			//Get kanji notes from the input note list with matching kanji ids
			List<Note> subKanjiNotes = GetNotesByKanjiIds(noteList, subKanjiIds);
			//Remove found notes from the input note list
			noteList = noteList.Except(subKanjiNotes).ToList();
			//Recursively call to grab any additional sub kanji notes based on the currently pulled kanji notes
			subKanjiNotes.AddRange(PullAllSubKanjiNotesFromNoteList(ref noteList, subKanjiNotes));
			//Return the full list of all related sub kanji notes
			return subKanjiNotes;
		}

		public bool MoveNotesBetweenDecks(IEnumerable<long> noteIds, long newDeckId)
		{
			//Grab all the cards (Note/Deck junction table) with the given note ids
			var existingCards = _context.Cards.Where(c => noteIds.Contains(c.NoteId));
			//Update the deck id for each card
			foreach (var card in existingCards)
			{
				card.DeckId = newDeckId;
			}
			//Save the changes
			_context.SaveChanges();
			//Return success
			return true;
		}

		public IEnumerable<long> GetNoteIdsWithAtLeastInterval(IEnumerable<long> noteIds, int interval)
		{
			return _context.Cards
						.Where(c => noteIds.Contains(c.NoteId))//Grab cards with matching note ids
						.Where(c => c.Interval >= interval)//Filter cards with matching intervals
						.Select(c => c.NoteId)//Grab the note ids
						.ToList();//Return the list
		}

		/// <summary>
		/// Get the minimum interval for each note in a list of note ids
		/// </summary>
		/// <param name="noteIds"></param>
		/// <returns>
		/// Dictionary of note ids to their minimum interval (if a card type that has duplicate notes)
		/// </returns>
		//public Dictionary<long, int> GetMinIntervalForEachNote(IEnumerable<long> noteIds)
		//{
		//	return null;
		//}

		public IEnumerable<long> GetNoteIdsWithAtLeastKanjiInterval(IEnumerable<long> noteIds)
		{
			//Get the minimum interval for moving newKanji into learningKanji
			int newKanjiInterval = AnkiBindingConfig.Bindings.NoteIntervalLimits.MoveFromNewKanji;
			//Return the note ids with the minimum interval
			return GetNoteIdsWithAtLeastInterval(noteIds, newKanjiInterval);
		}

		public bool MoveNewKanjiToLearningKanji()
		{
			//Get new kanji decks
			var newKanjiDecks = GetNewKanjiDecks();
			//Get the new kanji note ids
			var newKanjiNoteIds = newKanjiDecks.SelectMany(d => GetDeckNotes(d.Id)).Select(n => n.Id).ToList();
			//Get the new kanji note ids to be moved (based on the minimum interval)
			var newKanjiNoteIdsToMove = GetNoteIdsWithAtLeastKanjiInterval(newKanjiNoteIds);
			//Get the learning kanji decks
			var learningKanjiDecks = GetLearningKanjiDecks();
			//Fail if no learning kanji decks found
			if (!learningKanjiDecks.Any()) { return false; }
			//Get the first learning deck id to move the new kanji notes to
			var learningKanjiDeckId = learningKanjiDecks.First().Id;
			//Move the new kanji notes to the learning kanji deck
			return MoveNotesBetweenDecks(newKanjiNoteIdsToMove, learningKanjiDeckId);
		}

		public bool MoveResourceSubKanjiToNewKanji()
		{
			//Get the kanji resource decks
			var kanjiResourceDecks = GetResourceKanjiDecks();
			//Get the kanji resource notes
			var kanjiResourceNotes = kanjiResourceDecks.SelectMany(d => GetDeckNotes(d.Id)).ToList();
			//Get the new kanji decks
			var newKanjiDecks = GetNewKanjiDecks();
			//Fail if no new kanji decks found
			if (!newKanjiDecks.Any()) { return false; }
			//Get the new kanji notes
			var newKanjiNotes = newKanjiDecks.SelectMany(d => GetDeckNotes(d.Id)).ToList();
			//Pull kanji resource notes based on the new kanji sub kanji ids
			var SubKanjiResourceNotes = PullAllSubKanjiNotesFromNoteList(ref kanjiResourceNotes, newKanjiNotes);
			//Skip if no new kanji sub kanji notes to move
			if (!SubKanjiResourceNotes.Any()) { return true; }
			//Get the sub kanji resource note ids
			var subKanjiResourceNoteIdsToMove = SubKanjiResourceNotes.Select(n => n.Id).ToList();
			//Get the first new kanji deck id to move the resource kanji notes to
			var newKanjiDeckId = newKanjiDecks.First().Id;
			//Move the resource kanji notes to the new kanji deck
			return MoveNotesBetweenDecks(subKanjiResourceNoteIdsToMove, newKanjiDeckId);
		}
	}
}

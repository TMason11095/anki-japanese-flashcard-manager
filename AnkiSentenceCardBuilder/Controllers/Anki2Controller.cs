using AnkiJapaneseFlashcardManager.Config;
using AnkiSentenceCardBuilder.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiSentenceCardBuilder.Controllers
{
    public class Anki2Controller
    {
        private readonly Anki2Context _context;

        public Anki2Controller(Anki2Context context)
        {
            _context = context;
        }

        public Anki2Controller(string dbPath) : this(new Anki2Context(dbPath))
        {
        }
        
        public List<T> GetTable<T>() where T : class
        {
            return _context.Set<T>().ToList();
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

		public List<long> GetSubKanjiIds(Note kanjiNote)//TODO
		{
			return null;
		}

		public List<string> GetSubKanjiIds(List<Note> kanjiNotes)
		{
			//Get sub kanji id tag
			string subKanjiIdTag = AnkiBindingConfig.Bindings.NoteTags.SubKanjiId;
			//Return the sub kanji ids
			return kanjiNotes.SelectMany(n => n.TagsList)//Get all the note tags
				.Where(t => t.StartsWith(subKanjiIdTag))//Filter to find the sub kanji id tags
				.Select(t => t.Substring(subKanjiIdTag.Length))//Get the sub kanji ids
				.Distinct()//Filter out duplicate entries (Multiple kanji can share the same sub kanji id)
				.ToList();
		}
	}
}

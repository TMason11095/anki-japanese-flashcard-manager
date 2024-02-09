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

		public List<Note> GetDeckNotes(long deckId)//TODO
		{
			return null;
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
	}
}

using AnkiJapaneseFlashcardManager.Config;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
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
    public class Anki2Controller
	{
        private readonly Anki2Context _context;

        public Anki2Controller(Anki2Context context)
        {
            _context = context;
        }

		public List<T> GetTable<T>() where T : class
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

		//public Deck GetDeckById(long deckId)//TODO
		//{
		//	return null;
		//}

		public List<Note> GetDeckNotes(long deckId)//Card(Note)
		{
			//Return the notes from unique card entries with the given deck id
			return _context.Cards
					.Where(c => c.DeckId == deckId) //Grab cards with matching deck id
					.Select(c => c.Note) //Grab the notes
					.Distinct() //Filter out duplicate entries
					.ToList();
		}

		//public Note GetNoteById(long noteId)//TODO
		//{
		//	return null;
		//}

		//public List<long> GetSubKanjiIds(Note kanjiNote)//TODO
		//{
		//	return null;
		//}

		public bool MoveNotesBetweenDecks(IEnumerable<long> noteIds, long newDeckId)//Card(Note)
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

		public IEnumerable<long> GetNoteIdsWithAtLeastInterval(IEnumerable<long> noteIds, int interval)//Card(Note)
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

		public IEnumerable<long> GetNoteIdsWithAtLeastKanjiInterval(IEnumerable<long> noteIds)//Card(Note)
		{
			//Get the minimum interval for moving newKanji into learningKanji
			int newKanjiInterval = AnkiBindingConfig.Bindings.NoteIntervalLimits.MoveFromNewKanji;
			//Return the note ids with the minimum interval
			return GetNoteIdsWithAtLeastInterval(noteIds, newKanjiInterval);
		}

		
	}
}

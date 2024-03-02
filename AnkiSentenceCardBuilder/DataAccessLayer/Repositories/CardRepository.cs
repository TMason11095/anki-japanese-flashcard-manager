using AnkiJapaneseFlashcardManager.DataAccessLayer.Interfaces.Contexts;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories
{
	public class CardRepository
	{
		private readonly IAnki2Context _context;

		public CardRepository(IAnki2Context context)
		{
			_context = context;
		}

		public List<Note> GetDeckNotes(long deckId)//Card(Note)
		{
			//Return the notes from unique card entries with the given deck id
			return _context.Cards
					.Where(c => c.DeckId == deckId) //Grab cards with matching deck id
					.Select(c => c.Note) //Grab the notes
					.Distinct() //Filter out duplicate entries
					.ToList();
		}

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
	}
}

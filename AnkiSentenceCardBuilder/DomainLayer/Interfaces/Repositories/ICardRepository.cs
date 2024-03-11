using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.DomainLayer.Interfaces.Repositories
{
	public interface ICardRepository
	{
		IEnumerable<Note> GetDeckNotes(long deckId);
		bool MoveNotesBetweenDecks(IEnumerable<long> noteIds, long newDeckId);
		IEnumerable<long> GetNoteIdsWithAtLeastInterval(IEnumerable<long> noteIds, int interval);
	}
}

using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using AnkiJapaneseFlashcardManager.DomainLayer.Interfaces.Repositories;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Helpers
{
	public static class Anki2Extension
	{
		public static IEnumerable<long> GetIds(this IEnumerable<Note> notes)
		{
			return notes.Select(n => n.Id);
		}

		public static IEnumerable<long> GetIds(this IEnumerable<Deck> decks)
		{
			return decks.Select(d => d.Id);
		}

		public static IEnumerable<Note> GetNotes(this IEnumerable<Deck> decks, ICardRepository cardRepository)
		{
			return decks.SelectMany(d => cardRepository.GetDeckNotes(d.Id));
		}
	}
}

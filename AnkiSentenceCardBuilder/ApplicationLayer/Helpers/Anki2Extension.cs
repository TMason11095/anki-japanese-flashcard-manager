using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}

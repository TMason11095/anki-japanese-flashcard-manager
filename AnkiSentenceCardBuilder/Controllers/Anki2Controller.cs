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

		//public Note GetNoteById(long noteId)//TODO
		//{
		//	return null;
		//}

		//public List<long> GetSubKanjiIds(Note kanjiNote)//TODO
		//{
		//	return null;
		//}

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
	}
}

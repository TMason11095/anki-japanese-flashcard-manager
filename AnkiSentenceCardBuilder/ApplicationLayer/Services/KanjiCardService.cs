using AnkiJapaneseFlashcardManager.ApplicationLayer.Config;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
using AnkiJapaneseFlashcardManager.DomainLayer.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services
{
    public class KanjiCardService
	{
		private readonly ICardRepository _cardRepository;

		public KanjiCardService(ICardRepository cardRepository)
		{
			_cardRepository = cardRepository;
		}

		public IEnumerable<long> GetNoteIdsWithAtLeastKanjiInterval(IEnumerable<long> noteIds)//Card(Note)
		{
			//Get the minimum interval for moving newKanji into learningKanji
			int newKanjiInterval = AnkiBindingConfig.Bindings.NoteIntervalLimits.MoveFromNewKanji;
			//Return the note ids with the minimum interval
			return _cardRepository.GetNoteIdsWithAtLeastInterval(noteIds, newKanjiInterval);
		}
	}
}

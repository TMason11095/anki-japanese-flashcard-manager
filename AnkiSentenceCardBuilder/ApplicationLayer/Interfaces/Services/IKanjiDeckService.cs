using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Interfaces.Services
{
	public interface IKanjiDeckService
	{
		IEnumerable<Deck> GetResourceKanjiDecks();
		IEnumerable<Deck> GetNewKanjiDecks();
		IEnumerable<Deck> GetLearningKanjiDecks();
	}
}

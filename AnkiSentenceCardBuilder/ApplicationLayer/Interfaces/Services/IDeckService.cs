using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Interfaces.Services
{
	public interface IDeckService
	{
		IEnumerable<Deck> GetTaggedDecks(string deckTagName);
	}
}

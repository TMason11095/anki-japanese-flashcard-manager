using AnkiJapaneseFlashcardManager.ApplicationLayer.Config;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services
{
    public class KanjiDeckService
	{
		private readonly DeckService _deckService;

		public KanjiDeckService(DeckService deckService)
		{
			_deckService = deckService;
		}

		public IEnumerable<Deck> GetResourceKanjiDecks()//Deck
		{
			//Get resource kanji deck tag name
			string deckTagName = AnkiBindingConfig.Bindings.ResourceDecks.Kanji;
			//Return the decks
			return _deckService.GetTaggedDecks(deckTagName);
		}

		public IEnumerable<Deck> GetNewKanjiDecks()//Deck
		{
			//Get new kanji deck tag name
			string deckTagName = AnkiBindingConfig.Bindings.NewDecks.Kanji;
			//Return the decks
			return _deckService.GetTaggedDecks(deckTagName);
		}

		public IEnumerable<Deck> GetLearningKanjiDecks()//Deck
		{
			//Get new kanji deck tag name
			string deckTagName = AnkiBindingConfig.Bindings.LearningDecks.Kanji;
			//Return the decks
			return _deckService.GetTaggedDecks(deckTagName);
		}
	}
}

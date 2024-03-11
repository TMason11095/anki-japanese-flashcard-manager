using AnkiJapaneseFlashcardManager.ApplicationLayer.Config;
using AnkiJapaneseFlashcardManager.ApplicationLayer.Interfaces.Services;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services
{
    public class KanjiDeckService
	{
		private readonly IDeckService _deckService;

		public KanjiDeckService(IDeckService deckService)
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

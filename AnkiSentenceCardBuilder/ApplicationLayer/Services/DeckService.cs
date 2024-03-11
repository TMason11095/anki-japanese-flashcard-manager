using AnkiJapaneseFlashcardManager.ApplicationLayer.Config;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using AnkiJapaneseFlashcardManager.DomainLayer.Interfaces.Repositories;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services
{
    public class DeckService
	{
		private readonly IDeckRepository _deckRepository;

		public DeckService(IDeckRepository deckRepository)
		{
			_deckRepository = deckRepository;
		}

		public IEnumerable<Deck> GetTaggedDecks(string deckTagName)//Deck
		{
			//Get the deck tag (prefix for the full tag)
			string deckTag = AnkiBindingConfig.Bindings.DeckTag;
			//Build the full tag
			string fullDeckTag = deckTag + deckTagName;
			//Return the tagged decks
			return _deckRepository.GetDecksByDescriptionContaining(fullDeckTag);
		}
	}
}

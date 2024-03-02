using AnkiJapaneseFlashcardManager.Config;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services
{
	public class DeckService
	{
		private readonly DeckRepository _deckRepository;

		public DeckService(DeckRepository deckRepository)
		{
			_deckRepository = deckRepository;
		}

		public List<Deck> GetTaggedDecks(string deckTagName)//Deck
		{
			//Get the deck tag (prefix for the full tag)
			string deckTag = AnkiBindingConfig.Bindings.DeckTag;
			//Build the full tag
			string fullDeckTag = deckTag + deckTagName;
			//Return the tagged decks
			return _deckRepository.GetDecksByDescriptionContaining(fullDeckTag).ToList();
		}
	}
}

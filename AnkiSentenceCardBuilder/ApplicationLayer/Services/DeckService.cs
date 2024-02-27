using AnkiJapaneseFlashcardManager.Config;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services
{
	public class DeckService
	{
		private readonly Anki2Controller _anki2Controller;

		public DeckService(Anki2Controller anki2Controller)
		{
			_anki2Controller = anki2Controller;
		}

		public List<Deck> GetTaggedDecks(string deckTagName)//Deck
		{
			//Get the deck tag (prefix for the full tag)
			string deckTag = AnkiBindingConfig.Bindings.DeckTag;
			//Build the full tag
			string fullDeckTag = deckTag + deckTagName;
			//Return the tagged decks
			return _anki2Controller.GetDecksByDescriptionContaining(fullDeckTag).ToList();
		}
	}
}

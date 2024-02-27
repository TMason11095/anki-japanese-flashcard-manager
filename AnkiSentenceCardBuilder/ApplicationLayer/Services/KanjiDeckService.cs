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
	public class KanjiDeckService
	{
		private readonly Anki2Controller _anki2Controller;

		public KanjiDeckService(Anki2Controller anki2Controller)
		{
			_anki2Controller = anki2Controller;
		}

		public List<Deck> GetResourceKanjiDecks()//Deck
		{
			//Get resource kanji deck tag name
			string deckTagName = AnkiBindingConfig.Bindings.ResourceDecks.Kanji;
			//Return the decks
			return _anki2Controller.GetTaggedDecks(deckTagName);
		}
	}
}

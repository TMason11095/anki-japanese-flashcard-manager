using AnkiJapaneseFlashcardManager.ApplicationLayer.Services;
using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManagerTests.ApplicationLayer.Services
{
	public class KanjiDeckServiceTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		[InlineData("empty_kanjiResource_deck.anki2")]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2")]
		public void Get_kanji_resource_decks(string anki2File)//TODO: Refactor to check for expected values
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			KanjiDeckService kanjiDeckService = new KanjiDeckService(anki2Controller);

			//Act
			var taggedDecks = kanjiDeckService.GetResourceKanjiDecks();

			//Assert
			taggedDecks.Should().NotBeEmpty();
		}

		[Theory]
		[InlineData("empty_random_decks.anki2")]
		public void No_kanji_resource_decks_is_empty(string anki2File)
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			KanjiDeckService kanjiDeckService = new KanjiDeckService(anki2Controller);

			//Act
			var taggedDecks = kanjiDeckService.GetResourceKanjiDecks();

			//Assert
			taggedDecks.Should().BeEmpty();
		}
	}
}

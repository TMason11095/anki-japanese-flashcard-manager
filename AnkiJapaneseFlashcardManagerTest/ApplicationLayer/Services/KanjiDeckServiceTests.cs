using AnkiJapaneseFlashcardManager.ApplicationLayer.Services;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
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
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));
			KanjiDeckService kanjiDeckService = new KanjiDeckService(deckService);

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
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));
			KanjiDeckService kanjiDeckService = new KanjiDeckService(deckService);

			//Act
			var taggedDecks = kanjiDeckService.GetResourceKanjiDecks();

			//Assert
			taggedDecks.Should().BeEmpty();
		}

		[Theory]
		[InlineData("empty_newKanji_deck.anki2")]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2")]
		public void Get_new_kanji_decks(string anki2File)//TODO: Refactor to check for expected values
		{
			//Arange
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));
			KanjiDeckService kanjiDeckService = new KanjiDeckService(deckService);

			//Act
			var taggedDecks = kanjiDeckService.GetNewKanjiDecks();

			//Assert
			taggedDecks.Should().NotBeEmpty();
		}

		[Theory]
		[InlineData("empty_random_decks.anki2")]
		public void No_new_kanji_decks_is_empty(string anki2File)
		{
			//Arange
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));
			KanjiDeckService kanjiDeckService = new KanjiDeckService(deckService);

			//Act
			var learningKanjiDecks = kanjiDeckService.GetNewKanjiDecks();

			//Assert
			learningKanjiDecks.Should().BeEmpty();
		}

		[Theory]
		[InlineData("emptyLearningKanji_飲newKanji_食欠人良resourceKanji_decks.anki2")]
		public void Get_learning_kanji_decks(string anki2File)//TODO: Refactor to check for expected values
		{
			//Arange
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));
			KanjiDeckService kanjiDeckService = new KanjiDeckService(deckService);

			//Act
			var learningKanjiDecks = kanjiDeckService.GetLearningKanjiDecks();

			//Assert
			learningKanjiDecks.Should().NotBeEmpty();
		}

		[Theory]
		[InlineData("empty_random_decks.anki2")]
		public void No_learning_kanji_decks_is_empty(string anki2File)
		{
			//Arange
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));
			KanjiDeckService kanjiDeckService = new KanjiDeckService(deckService);

			//Act
			var learningKanjiDecks = kanjiDeckService.GetLearningKanjiDecks();

			//Assert
			learningKanjiDecks.Should().BeEmpty();
		}
	}
}

using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManagerTests
{
	public class Anki2DeckControllerTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		[InlineData("empty_kanjiResource_deck.anki2", "KanjiResource")]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", "KanjiResource")]
		[InlineData("empty_random_decks.anki2", "Random")]
		[InlineData("empty_random_decks.anki2", "RandomResource")]
		public void Get_decks_by_description_tag(string anki2File, string deckTagName)//TODO: Refactor to check for expected values
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var taggedDecks = anki2Controller.GetTaggedDecks(deckTagName);

			//Assert
			taggedDecks.Should().NotBeEmpty();
		}

		[Theory]
		[InlineData("empty_kanjiResource_deck.anki2", "NonExistentTag")]
		[InlineData("empty_random_decks.anki2", "KanjiResource")]
		public void No_decks_with_matching_description_tag_is_empty(string anki2File, string deckTagName)
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var taggedDecks = anki2Controller.GetTaggedDecks(deckTagName);

			//Assert
			taggedDecks.Should().BeEmpty();
		}

		[Theory]
		[InlineData("empty_kanjiResource_deck.anki2")]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2")]
		public void Get_kanji_resource_decks(string anki2File)//TODO: Refactor to check for expected values
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var taggedDecks = anki2Controller.GetResourceKanjiDecks();

			//Assert
			taggedDecks.Should().NotBeEmpty();
		}

		[Theory]
		[InlineData("empty_random_decks.anki2")]
		public void No_kanji_resource_decks_is_empty(string anki2File)
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var taggedDecks = anki2Controller.GetResourceKanjiDecks();

			//Assert
			taggedDecks.Should().BeEmpty();
		}

		[Theory]
		[InlineData("empty_newKanji_deck.anki2")]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2")]
		public void Get_new_kanji_decks(string anki2File)//TODO: Refactor to check for expected values
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var taggedDecks = anki2Controller.GetNewKanjiDecks();

			//Assert
			taggedDecks.Should().NotBeEmpty();
		}

		[Theory]
		[InlineData("empty_random_decks.anki2")]
		public void No_new_kanji_decks_is_empty(string anki2File)
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var learningKanjiDecks = anki2Controller.GetNewKanjiDecks();

			//Assert
			learningKanjiDecks.Should().BeEmpty();
		}

		[Theory]
		[InlineData("emptyLearningKanji_飲newKanji_食欠人良resourceKanji_decks.anki2")]
		public void Get_learning_kanji_decks(string anki2File)//TODO: Refactor to check for expected values
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var learningKanjiDecks = anki2Controller.GetLearningKanjiDecks();

			//Assert
			learningKanjiDecks.Should().NotBeEmpty();
		}

		[Theory]
		[InlineData("empty_random_decks.anki2")]
		public void No_learning_kanji_decks_is_empty(string anki2File)
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var learningKanjiDecks = anki2Controller.GetLearningKanjiDecks();

			//Assert
			learningKanjiDecks.Should().BeEmpty();
		}
	}
}

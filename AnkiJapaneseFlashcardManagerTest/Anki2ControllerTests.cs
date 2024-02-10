using AnkiSentenceCardBuilder.Controllers;
using FluentAssertions;

namespace AnkiJapaneseFlashcardManagerTest
{
	public class Anki2ControllerTests
	{
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		[InlineData("empty_kanjiResource_deck.anki2", "KanjiResource")]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", "KanjiResource")]
		[InlineData("empty_random_decks.anki2", "Random")]
		[InlineData("empty_random_decks.anki2", "RandomResource")]
		public void Get_decks_by_description_tag(string anki2File, string deckTagName)
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
		public void Get_kanji_resource_decks(string anki2File)
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
		public void Get_new_kanji_decks(string anki2File)
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
		public void Get_learning_kanji_decks(string anki2File)
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

		[Theory]
		[InlineData("empty_random_decks.anki2", 1706982318565)]
		[InlineData("empty_random_decks.anki2", 1706982351536)]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", 1706982318565)]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", 1706982351536)]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", 1707160682667)]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", 1707160947123)]
		public void No_notes_found_in_a_deck_is_empty(string anki2File, long deckId)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var notes = anki2Controller.GetDeckNotes(deckId);

			//Assert
			notes.Should().BeEmpty();
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123)]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667)]
		public void Get_notes_by_deck_id(string anki2File, long deckId)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var notes = anki2Controller.GetDeckNotes(deckId);

			//Assert
			notes.Should().NotBeEmpty();
		}

		[Theory]
		[InlineData("deck_with_different_card_types.anki2", 1707263514556, new[] { 1707263555296, 1707263973429, 1707263567670 })]
		public void Mulitple_card_entries_of_the_same_note_is_a_distinct_list_of_notes(string anki2File, long deckId, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var notes = anki2Controller.GetDeckNotes(deckId);

			//Assert
			notes.Select(n => n.Id).Should().BeEquivalentTo(expectedNoteIds);
		}
	}
}
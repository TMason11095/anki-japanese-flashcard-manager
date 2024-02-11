using AnkiSentenceCardBuilder.Controllers;
using AnkiSentenceCardBuilder.Models;
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

		//[Theory]
		//[InlineData("empty_kanjiResource_deck.anki2", 1706982246215)]
		//[InlineData("empty_kanjiResource_newKanji_decks.anki2", 1707160947123)]
		//[InlineData("empty_random_decks.anki2", 1706982351536)]
		//[InlineData("empty_random_decks.anki2", 1706982318565)]
		//public void Get_deck_by_id(string anki2File, long deckId)
		//{
		//	//Arange
		//	Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

		//	//Act
		//	var deck = anki2Controller.GetDeckById(deckId);

		//	//Assert
		//	deck.Should().NotBeNull();
		//}

		//[Theory]
		//[InlineData("empty_kanjiResource_deck.anki2", 0000000000000)]
		//[InlineData("empty_random_decks.anki2", 0000000000000)]
		//public void No_deck_by_id_is_null(string anki2File, long deckId)
		//{
		//	//Arange
		//	Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

		//	//Act
		//	var deck = anki2Controller.GetDeckById(deckId);

		//	//Assert
		//	deck.Should().BeNull();
		//}

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
		public void Get_notes_by_deck_id(string anki2File, long deckId)//TODO: Refactor to check for expected values
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

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, "kid:", new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 })]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, "kid:", new[] { 1707169522144 })]
		public void Get_notes_by_note_tag(string anki2File, long deckId, string noteTagName, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> notes = anki2Controller.GetDeckNotes(deckId);

			//Act
			var taggedNotes = anki2Controller.GetTaggedNotes(notes, noteTagName);

			//Assert
			taggedNotes.Select(n => n.Id).Should().BeEquivalentTo(expectedNoteIds);
		}

		[Theory]
		[InlineData("deck_with_different_card_types.anki2", 1707263514556, "kid:")]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, "nonExistentTag:")]
		public void No_tagged_notes_found_is_empty(string anki2File, long deckId, string noteTagName)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> notes = anki2Controller.GetDeckNotes(deckId);

			//Act
			var taggedNotes = anki2Controller.GetTaggedNotes(notes, noteTagName);

			//Assert
			taggedNotes.Should().BeEmpty();
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 })]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, new[] { 1707169522144 })]
		public void Get_kanji_notes(string anki2File, long deckId, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> notes = anki2Controller.GetDeckNotes(deckId);

			//Act
			var taggedNotes = anki2Controller.GetKanjiNotes(notes);

			//Assert
			taggedNotes.Select(n => n.Id).Should().BeEquivalentTo(expectedNoteIds);
		}

		[Theory]
		[InlineData("deck_with_different_card_types.anki2", 1707263514556)]
		public void No_kanji_notes_found_is_empty(string anki2File, long deckId)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> notes = anki2Controller.GetDeckNotes(deckId);

			//Act
			var taggedNotes = anki2Controller.GetKanjiNotes(notes);

			//Assert
			taggedNotes.Should().BeEmpty();
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, new[] { 1472, 466 })]
		public void Get_sub_kanji_ids_from_notes(string anki2File, long deckId, int[] expectedSubKanjiIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> deckNotes = anki2Controller.GetDeckNotes(deckId);
			List<Note> kanjiNotes = anki2Controller.GetKanjiNotes(deckNotes);

			//Act
			List<long> subKanjiIds = anki2Controller.GetSubKanjiIds(kanjiNotes);

			//Assert
			subKanjiIds.Should().BeEquivalentTo(expectedSubKanjiIds);
		}

		[Theory]
		[InlineData("deck_with_different_card_types.anki2", 1707263514556)]
		public void No_sub_kanji_ids_found_is_empty(string anki2File, long deckId)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> deckNotes = anki2Controller.GetDeckNotes(deckId);
			List<Note> kanjiNotes = anki2Controller.GetKanjiNotes(deckNotes);

			//Act
			List<long> subKanjiIds = anki2Controller.GetSubKanjiIds(kanjiNotes);

			//Assert
			subKanjiIds.Should().BeEmpty();
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, new[] { 1468, 951 })]
		public void Duplicate_sub_kanji_ids_found_is_a_distinct_list(string anki2File, long deckId, int[] expectedSubKanjiIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> deckNotes = anki2Controller.GetDeckNotes(deckId);
			List<Note> kanjiNotes = anki2Controller.GetKanjiNotes(deckNotes);

			//Act
			List<long> subKanjiIds = anki2Controller.GetSubKanjiIds(kanjiNotes);

			//Assert
			subKanjiIds.Should().BeEquivalentTo(expectedSubKanjiIds);
		}
	}
}
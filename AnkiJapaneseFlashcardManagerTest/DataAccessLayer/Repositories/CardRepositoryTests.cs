using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManagerTests.DataAccessLayer.Repositories
{
	public class CardRepositoryTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

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
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			Anki2Controller anki2Controller = new Anki2Controller(dbContext);
			CardRepository cardRepo = new CardRepository(dbContext);

			//Act
			var notes = cardRepo.GetDeckNotes(deckId);

			//Assert
			notes.Should().BeEmpty();
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123)]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667)]
		public void Get_notes_by_deck_id(string anki2File, long deckId)//TODO: Refactor to check for expected values
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			Anki2Controller anki2Controller = new Anki2Controller(dbContext);
			CardRepository cardRepo = new CardRepository(dbContext);

			//Act
			var notes = cardRepo.GetDeckNotes(deckId);

			//Assert
			notes.Should().NotBeEmpty();
		}

		[Theory]
		[InlineData("deck_with_different_card_types.anki2", 1707263514556, new[] { 1707263555296, 1707263973429, 1707263567670 })]
		public void Mulitple_card_entries_of_the_same_note_is_a_distinct_list_of_notes(string anki2File, long deckId, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			Anki2Controller anki2Controller = new Anki2Controller(dbContext);
			CardRepository cardRepo = new CardRepository(dbContext);

			//Act
			var notes = cardRepo.GetDeckNotes(deckId);

			//Assert
			notes.Select(n => n.Id).Should().BeEquivalentTo(expectedNoteIds);
		}
	}
}

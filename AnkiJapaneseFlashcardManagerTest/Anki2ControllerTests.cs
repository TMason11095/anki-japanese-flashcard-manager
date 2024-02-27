using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using AnkiSentenceCardBuilder.Controllers;

namespace AnkiJapaneseFlashcardManagerTests
{
    public class Anki2ControllerTests
	{
		string _anki2FolderPath = "./Resources/Anki2 Files/";

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


		//public void Get_minimum_interval_for_each_note(string anki2File, long[] noteIds, int[] expectedIntervals)
		//{
		//	//Arrange
		//	noteIds.Length.Should().Be(expectedIntervals.Length);//Input/output arrays should be the same length
		//	Dictionary<long, int> expectedNoteIntervals = new Dictionary<long, int>();//Setup the expected dictionary
		//	for (int i = 0; i < noteIds.Length; i++)//Fill the dictionary using the input note ids and the expected output intervals
		//	{
		//		expectedNoteIntervals[noteIds[i]] = expectedIntervals[i];
		//	}
		//	Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

		//	//Act
		//	var noteIntervals = anki2Controller.GetMinIntervalForEachNote(noteIds);

		//	//Assert
		//	noteIntervals.Count().Should().Be(expectedIntervals.Length);//Match length to original array in case the array had duplicate note ids
		//	noteIntervals.Should().BeEquivalentTo(expectedNoteIntervals);
		//}

		

		

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 }, 1707160947123, 1707160682667)]
		public void Move_resource_sub_kanji_notes_to_new_kanji_deck(string anki2File, long[] expectedNoteIdsToMove, long expectedFromDeckId, long expectedToDeckId)
		{
			//Arrange
			string originalInputFilePath = _anki2FolderPath + anki2File;
			string tempInputFilePath = _anki2FolderPath + "temp_" + anki2File;
			File.Copy(originalInputFilePath, tempInputFilePath, true);//Copy the input file to prevent changes between unit tests
			Anki2Controller anki2Controller = new Anki2Controller(tempInputFilePath);
			List<Card> allOriginalCards = anki2Controller.GetTable<Card>();

			//Act
			bool movedNotes = anki2Controller.MoveResourceSubKanjiToNewKanji();

			//Get Assert Values
			//Get changed cards
			var allCardsAfterFunction = anki2Controller.GetTable<Card>();
			var changedCards = allOriginalCards
				.Join(allCardsAfterFunction,
					originalCard => originalCard.Id,
					updatedCard => updatedCard.Id,
					(originalCard, updatedCard) => new { OriginalCard = originalCard, UpdatedCard = updatedCard })
				.Where(pair => !pair.OriginalCard.Equals(pair.UpdatedCard))
				.ToList();
			//Assert
			movedNotes.Should().BeTrue();//Function completed successfully
			changedCards.Select(p => p.OriginalCard.DeckId).Should().AllBeEquivalentTo(expectedFromDeckId);//Original deck id should match
			changedCards.Select(p => p.OriginalCard.NoteId).Distinct().Should().BeEquivalentTo(expectedNoteIdsToMove);//Original notes should match
			changedCards.Select(p => p.UpdatedCard.DeckId).Should().AllBeEquivalentTo(expectedToDeckId);//Updated deck id should match
			changedCards.Select(p => p.UpdatedCard.NoteId).Distinct().Should().BeEquivalentTo(expectedNoteIdsToMove);//Updated notes should match

			//Cleanup
			anki2Controller.Dispose();
			File.Delete(tempInputFilePath);
		}
	}
}
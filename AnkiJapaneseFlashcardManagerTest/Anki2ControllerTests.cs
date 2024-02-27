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

		

		

		
	}
}
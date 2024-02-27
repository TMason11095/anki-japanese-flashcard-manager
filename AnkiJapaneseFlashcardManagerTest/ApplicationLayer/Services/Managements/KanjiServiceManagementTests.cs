using AnkiJapaneseFlashcardManager.ApplicationLayer.Services.Managements;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManagerTests.ApplicationLayer.Services.Managements
{
	public class KanjiServiceManagementTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		[InlineData("emptyLearningKanji_1ivl飲12ivl良monthsIvl食欠人newKanji_decks.anki2", new[] { 1548988102030, 1552619440878, 1559353225229, 1559353240186 }, 1670499333507, 1708110494009)]
		public void Move_new_kanji_notes_to_learning_kanji_deck(string anki2File, long[] expectedNoteIdsToMove, long expectedFromDeckId, long expectedToDeckId)
		{
			//Arrange
			string originalInputFilePath = _anki2FolderPath + anki2File;
			string tempInputFilePath = _anki2FolderPath + "temp_" + anki2File;
			File.Copy(originalInputFilePath, tempInputFilePath, true);//Copy the input file to prevent changes between unit tests
			Anki2Controller anki2Controller = new Anki2Controller(tempInputFilePath);
			List<Card> allOriginalCards = anki2Controller.GetTable<Card>();
			KanjiServiceManagement kanjiServiceManagement = new KanjiServiceManagement(anki2Controller);

			//Act
			bool movedNotes = kanjiServiceManagement.MoveNewKanjiToLearningKanji();

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
			KanjiServiceManagement kanjiServiceManagement = new KanjiServiceManagement(anki2Controller);

			//Act
			bool movedNotes = kanjiServiceManagement.MoveResourceSubKanjiToNewKanji();

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

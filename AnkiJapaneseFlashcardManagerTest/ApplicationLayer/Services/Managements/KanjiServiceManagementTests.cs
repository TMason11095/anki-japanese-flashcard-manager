using AnkiJapaneseFlashcardManager.ApplicationLayer.Services;
using AnkiJapaneseFlashcardManager.ApplicationLayer.Services.Managements;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Helpers;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Interfaces.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.TestHelpers;

namespace Tests.ApplicationLayer.Services.Managements
{
	public class KanjiServiceManagementTests
	{
		[Theory]
		[InlineData("emptyLearningKanji_1ivl飲12ivl良monthsIvl食欠人newKanji_decks.anki2", new[] { 1548988102030, 1552619440878, 1559353225229, 1559353240186 }, 1670499333507, 1708110494009)]
		public void Move_new_kanji_notes_to_learning_kanji_deck(string anki2File, long[] expectedNoteIdsToMove, long expectedFromDeckId, long expectedToDeckId)
		{
			//Arrange
			Anki2TestHelper helper = new Anki2TestHelper(anki2File, createTempCopy: true);
			List<Card> allOriginalCards = helper.GetAllNoTrackingCards();
			KanjiServiceManagement kanjiServiceManagement = helper.KanjiServiceManagement;

			//Act
			bool movedNotes = kanjiServiceManagement.MoveNewKanjiToLearningKanji();

			//Get Assert Values
			//Get changed cards
			var allCardsAfterFunction = helper.GetAllNoTrackingCards();
			var changedCards = BeforeAfterCard.GetChangedCards(allOriginalCards, allCardsAfterFunction);
			//Assert
			movedNotes.Should().BeTrue();//Function completed successfully
			changedCards.Select(p => p.BeforeCard.DeckId).Should().AllBeEquivalentTo(expectedFromDeckId);//Original deck id should match
			changedCards.Select(p => p.BeforeCard.NoteId).Distinct().Should().BeEquivalentTo(expectedNoteIdsToMove);//Original notes should match
			changedCards.Select(p => p.AfterCard.DeckId).Should().AllBeEquivalentTo(expectedToDeckId);//Updated deck id should match
			changedCards.Select(p => p.AfterCard.NoteId).Distinct().Should().BeEquivalentTo(expectedNoteIdsToMove);//Updated notes should match
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 }, 1707160947123, 1707160682667)]
		public void Move_resource_sub_kanji_notes_to_new_kanji_deck(string anki2File, long[] expectedNoteIdsToMove, long expectedFromDeckId, long expectedToDeckId)
		{
			//Arrange
			Anki2TestHelper helper = new Anki2TestHelper(anki2File, createTempCopy: true);
			List<Card> allOriginalCards = helper.GetAllNoTrackingCards();
			KanjiServiceManagement kanjiServiceManagement = helper.KanjiServiceManagement;

			//Act
			bool movedNotes = kanjiServiceManagement.MoveResourceSubKanjiToNewKanji();

			//Get Assert Values
			//Get changed cards
			var allCardsAfterFunction = helper.GetAllNoTrackingCards();
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
		}
	}
}

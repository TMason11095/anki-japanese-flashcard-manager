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

namespace Tests.DataAccessLayer.Repositories
{
	public class CardRepositoryTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		//Test case: Note ids found
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, new long[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 })]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, new long[] { 1707169522144 })]
		//Test case: No note ids found
		[InlineData("empty_random_decks.anki2", 1706982318565, new long[] { })]
		[InlineData("empty_random_decks.anki2", 1706982351536, new long[] { })]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", 1706982318565, new long[] { })]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", 1706982351536, new long[] { })]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", 1707160682667, new long[] { })]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", 1707160947123, new long[] { })]
		//Test case: Duplicate note ids found is a distinct list
		[InlineData("deck_with_different_card_types.anki2", 1707263514556, new[] { 1707263555296, 1707263973429, 1707263567670 })]
		public void Get_notes_by_deck_id(string anki2File, long deckId, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			CardRepository cardRepo = new CardRepository(dbContext);

			//Act
			var notes = cardRepo.GetDeckNotes(deckId);

			//Assert
			notes.Select(n => n.Id).Should().BeEquivalentTo(expectedNoteIds);
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 }, 1707160682667)]
		public void Move_notes_between_decks(string anki2File, long[] noteIdsToMove, long deckIdToMoveTo)
		{
			//Arrange
			string originalInputFilePath = _anki2FolderPath + anki2File;
			string tempInputFilePath = $"{_anki2FolderPath}temp_{Guid.NewGuid()}.anki2";
			File.Copy(originalInputFilePath, tempInputFilePath, true);//Copy the input file to prevent changes between unit tests
			Anki2Context dbContext = new Anki2Context(tempInputFilePath);
			CardRepository cardRepo = new CardRepository(dbContext);
			List<Card> originalNoteDeckJunctions = dbContext.Cards.AsNoTracking()
																.Where(c => noteIdsToMove.Contains(c.NoteId))
																.ToList();//Grab the current note/deck relations for the give note ids

			//Act
			bool movedNotes = cardRepo.MoveNotesBetweenDecks(noteIdsToMove, deckIdToMoveTo);

			//Assert
			movedNotes.Should().BeTrue();//Function completed successfully
			List<Card> finalNoteDeckJunctions = dbContext.Cards.AsNoTracking()
																.Where(c => noteIdsToMove.Contains(c.NoteId))
																.ToList();//Grab the current note/deck relations for the give note ids after running the function
			finalNoteDeckJunctions.Count().Should().Be(originalNoteDeckJunctions.Count());//No note/deck relations should have been removed/added
			finalNoteDeckJunctions.Select(c => c.DeckId).Should().AllBeEquivalentTo(deckIdToMoveTo);//All junction deckIds should be the given deckId

			//Cleanup
			DbContextHelper.ClearSqlitePool(dbContext);
			File.Delete(tempInputFilePath);
		}

		[Theory]
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 1, new[] { 1707169497960, 1707169570657, 1707170000793, 1707169983389 })]
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 4, new[] { 1707169983389 })]
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 5, new[] { 1707169983389 })]
		public void Get_note_ids_with_at_least_the_given_interval(string anki2File, long[] noteIds, int interval, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			CardRepository cardRepo = new CardRepository(dbContext);

			//Act
			var noteIdsWithGivenInterval = cardRepo.GetNoteIdsWithAtLeastInterval(noteIds, interval);

			//Assert
			noteIdsWithGivenInterval.Should().BeEquivalentTo(expectedNoteIds);
		}

		[Theory]
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 7)]
		public void No_note_ids_found_with_at_least_the_given_interval_is_empty(string anki2File, long[] noteIds, int interval)//Refactor: Merge with Get_note_ids_with_at_least_the_given_interval()
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			CardRepository cardRepo = new CardRepository(dbContext);

			//Act
			var noteIdsWithGivenInterval = cardRepo.GetNoteIdsWithAtLeastInterval(noteIds, interval);

			//Assert
			noteIdsWithGivenInterval.Should().BeEmpty();
		}
	}
}

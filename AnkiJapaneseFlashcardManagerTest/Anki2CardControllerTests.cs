﻿using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManagerTests
{
    public class Anki2CardControllerTests
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
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 }, 1707160682667)]
		public void Move_notes_between_decks(string anki2File, long[] noteIdsToMove, long deckIdToMoveTo)
		{
			//Arrange
			string originalInputFilePath = _anki2FolderPath + anki2File;
			string tempInputFilePath = _anki2FolderPath + "temp_" + anki2File;
			File.Copy(originalInputFilePath, tempInputFilePath, true);//Copy the input file to prevent changes between unit tests
			Anki2Controller anki2Controller = new Anki2Controller(tempInputFilePath);
			List<Card> originalNoteDeckJunctions = anki2Controller.GetTable<Card>()
																.Where(c => noteIdsToMove.Contains(c.NoteId))
																.ToList();//Grab the current note/deck relations for the give note ids

			//Act
			bool movedNotes = anki2Controller.MoveNotesBetweenDecks(noteIdsToMove, deckIdToMoveTo);

			//Assert
			movedNotes.Should().BeTrue();//Function completed successfully
			List<Card> finalNoteDeckJunctions = anki2Controller.GetTable<Card>()
																.Where(c => noteIdsToMove.Contains(c.NoteId))
																.ToList();//Grab the current note/deck relations for the give note ids after running the function
			finalNoteDeckJunctions.Count().Should().Be(originalNoteDeckJunctions.Count());//No note/deck relations should have been removed/added
			finalNoteDeckJunctions.Select(c => c.DeckId).Should().AllBeEquivalentTo(deckIdToMoveTo);//All junction deckIds should be the given deckId

			//Cleanup
			anki2Controller.Dispose();
			File.Delete(tempInputFilePath);
		}

		[Theory]
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 1, new[] { 1707169497960, 1707169570657, 1707170000793, 1707169983389 })]
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 4, new[] { 1707169983389 })]
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 5, new[] { 1707169983389 })]
		public void Get_note_ids_with_at_least_the_given_interval(string anki2File, long[] noteIds, int interval, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var noteIdsWithGivenInterval = anki2Controller.GetNoteIdsWithAtLeastInterval(noteIds, interval);

			//Assert
			noteIdsWithGivenInterval.Should().BeEquivalentTo(expectedNoteIds);
		}

		[Theory]
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 7)]
		public void No_note_ids_found_with_at_least_the_given_interval_is_empty(string anki2File, long[] noteIds, int interval)//Refactor: Merge with Get_note_ids_with_at_least_the_given_interval()
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var noteIdsWithGivenInterval = anki2Controller.GetNoteIdsWithAtLeastInterval(noteIds, interval);

			//Assert
			noteIdsWithGivenInterval.Should().BeEmpty();
		}

		[Theory]
		//Test case: Note ids found
		[InlineData("emptyLearningKanji_1ivl飲12ivl良monthsIvl食欠人newKanji_decks.anki2", new[] { 1548988102030, 1552619440878, 1559353225229, 1559353240186, 1559353264106 }, new[] { 1548988102030, 1552619440878, 1559353225229, 1559353240186 })]
		//Test case: No note ids found
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, new long[0])]
		public void Get_note_ids_with_at_least_the_kanji_interval(string anki2File, long[] noteIds, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var noteIdsWithKanjiInterval = anki2Controller.GetNoteIdsWithAtLeastKanjiInterval(noteIds);

			//Assert
			noteIdsWithKanjiInterval.Should().BeEquivalentTo(expectedNoteIds);
		}
	}
}

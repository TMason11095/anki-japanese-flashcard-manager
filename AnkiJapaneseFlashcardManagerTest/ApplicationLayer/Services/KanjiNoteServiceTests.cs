using AnkiJapaneseFlashcardManager.ApplicationLayer.Services;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ApplicationLayer.Services
{
	public class KanjiNoteServiceTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		//Test Case: Note ids found
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 })]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, new[] { 1707169522144 })]
		//Test Case: Note ids not found
		[InlineData("deck_with_different_card_types.anki2", 1707263514556, new long[0])]
		public void Get_kanji_notes(string anki2File, long deckId, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			CardRepository cardRepo = new CardRepository(dbContext);
			List<Note> notes = cardRepo.GetDeckNotes(deckId);
			KanjiNoteService kanjiNoteService = new KanjiNoteService();

			//Act
			var taggedNotes = kanjiNoteService.GetKanjiNotes(notes);

			//Assert
			taggedNotes.Select(n => n.Id).Should().BeEquivalentTo(expectedNoteIds);
		}

		[Theory]
		//Test case: Sub kanji ids found
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, new[] { "1472", "466" })]
		//Test case: Sub kanji ids not found
		[InlineData("deck_with_different_card_types.anki2", 1707263514556, new string[0])]
		//Test case: Duplicate sub kanji ids found is a distinct list
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, new[] { "1468", "951" })]
		public void Get_sub_kanji_ids_from_notes(string anki2File, long deckId, string[] expectedSubKanjiIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			CardRepository cardRepo = new CardRepository(dbContext);
			List<Note> deckNotes = cardRepo.GetDeckNotes(deckId);
			KanjiNoteService kanjiNoteService = new KanjiNoteService();
			List<Note> kanjiNotes = kanjiNoteService.GetKanjiNotes(deckNotes);

			//Act
			List<string> subKanjiIds = kanjiNoteService.GetSubKanjiIds(kanjiNotes);

			//Assert
			subKanjiIds.Should().BeEquivalentTo(expectedSubKanjiIds);
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, 1707160682667, new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 })]
		public void Pull_all_sub_kanji_notes_from_note_list(string anki2File, long sourceDeckId, long originalKanjiDeckId, long[] expectedKanjiNoteIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			KanjiNoteService kanjiNoteService = new KanjiNoteService();
			CardRepository cardRepo = new CardRepository(dbContext);
			List<Note> sourceNotes = cardRepo.GetDeckNotes(sourceDeckId);
			int sourceNotesOriginalCount = sourceNotes.Count;
			List<Note> originalKanjiNotes = cardRepo.GetDeckNotes(originalKanjiDeckId);

			//Act
			var subKanjiNotes = kanjiNoteService.PullAllSubKanjiNotesFromNoteList(ref sourceNotes, originalKanjiNotes);

			//Assert
			sourceNotes.Count.Should().Be(sourceNotesOriginalCount - subKanjiNotes.Count);//Should have removed all the found kanji notes
			subKanjiNotes.Select(n => n.Id).Should().BeEquivalentTo(expectedKanjiNoteIds);
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1706982318565, 1707160682667, new long[0])]
		public void Sub_kanji_notes_not_found_in_note_list_is_empty(string anki2File, long sourceDeckId, long originalKanjiDeckId, long[] expectedKanjiNoteIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			KanjiNoteService kanjiNoteService = new KanjiNoteService();
			CardRepository cardRepo = new CardRepository(dbContext);
			List<Note> sourceNotes = cardRepo.GetDeckNotes(sourceDeckId);
			int sourceNotesOriginalCount = sourceNotes.Count;
			List<Note> originalKanjiNotes = cardRepo.GetDeckNotes(originalKanjiDeckId);

			//Act
			var subKanjiNotes = kanjiNoteService.PullAllSubKanjiNotesFromNoteList(ref sourceNotes, originalKanjiNotes);

			//Assert
			sourceNotes.Count.Should().Be(sourceNotesOriginalCount - subKanjiNotes.Count);//Should have removed all the found kanji notes
			subKanjiNotes.Select(n => n.Id).Should().BeEquivalentTo(expectedKanjiNoteIds);
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, "kid:", new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 })]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, "kid:", new[] { 1707169522144 })]
		public void Get_notes_by_note_tag(string anki2File, long deckId, string noteTagName, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			KanjiNoteService kanjiNoteService = new KanjiNoteService();
			CardRepository cardRepo = new CardRepository(dbContext);
			List<Note> notes = cardRepo.GetDeckNotes(deckId);

			//Act
			var taggedNotes = kanjiNoteService.GetTaggedNotes(notes, noteTagName);

			//Assert
			taggedNotes.Select(n => n.Id).Should().BeEquivalentTo(expectedNoteIds);
		}

		[Theory]
		[InlineData("deck_with_different_card_types.anki2", 1707263514556, "kid:")]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, "nonExistentTag:")]
		public void No_tagged_notes_found_is_empty(string anki2File, long deckId, string noteTagName)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			KanjiNoteService kanjiNoteService = new KanjiNoteService();
			CardRepository cardRepo = new CardRepository(dbContext);
			List<Note> notes = cardRepo.GetDeckNotes(deckId);

			//Act
			var taggedNotes = kanjiNoteService.GetTaggedNotes(notes, noteTagName);

			//Assert
			taggedNotes.Should().BeEmpty();
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, new[] { "1474" }, new[] { 1707169522144 })]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, new[] { "1472", "466", "951", "1468" }, new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 })]
		public void Get_notes_by_kanji_ids(string anki2File, long deckId, string[] kanjiIds, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			KanjiNoteService kanjiNoteService = new KanjiNoteService();
			CardRepository cardRepo = new CardRepository(dbContext);
			List<Note> notes = cardRepo.GetDeckNotes(deckId);

			//Redundant? (Filter for "kid:" but then instantly filter again to grab the ids)
			//(Only care about using GetKanjiNotes() when trying to access data that isn't "kid")
			//List<Note> kanjiNotes = anki2Controller.GetKanjiNotes(notes);

			//Act
			List<Note> kanjiNotes = kanjiNoteService.GetNotesByKanjiIds(notes, kanjiIds);

			//Assert
			kanjiNotes.Select(n => n.Id).Should().BeEquivalentTo(expectedNoteIds);
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, new[] { "0", "nonExistentId" })]
		public void Invalid_kanji_id_is_empty(string anki2File, long deckId, string[] kanjiIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			KanjiNoteService kanjiNoteService = new KanjiNoteService();
			CardRepository cardRepo = new CardRepository(dbContext);
			List<Note> notes = cardRepo.GetDeckNotes(deckId);

			//Act
			List<Note> kanjiNotes = kanjiNoteService.GetNotesByKanjiIds(notes, kanjiIds);

			//Assert
			kanjiNotes.Should().BeEmpty();
		}
	}
}

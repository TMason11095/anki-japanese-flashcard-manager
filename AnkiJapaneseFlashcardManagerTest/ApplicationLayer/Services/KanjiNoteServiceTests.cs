using AnkiJapaneseFlashcardManager.ApplicationLayer.Services;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManagerTests.ApplicationLayer.Services
{
	public class KanjiNoteServiceTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 })]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, new[] { 1707169522144 })]
		public void Get_kanji_notes(string anki2File, long deckId, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> notes = anki2Controller.GetDeckNotes(deckId);
			KanjiNoteService kanjiNoteService = new KanjiNoteService(anki2Controller);

			//Act
			var taggedNotes = kanjiNoteService.GetKanjiNotes(notes);

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
			KanjiNoteService kanjiNoteService = new KanjiNoteService(anki2Controller);

			//Act
			var taggedNotes = kanjiNoteService.GetKanjiNotes(notes);

			//Assert
			taggedNotes.Should().BeEmpty();
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, new[] { "1472", "466" })]
		public void Get_sub_kanji_ids_from_notes(string anki2File, long deckId, string[] expectedSubKanjiIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> deckNotes = anki2Controller.GetDeckNotes(deckId);
			KanjiNoteService kanjiNoteService = new KanjiNoteService(anki2Controller);
			List<Note> kanjiNotes = kanjiNoteService.GetKanjiNotes(deckNotes);

			//Act
			List<string> subKanjiIds = anki2Controller.GetSubKanjiIds(kanjiNotes);

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
			KanjiNoteService kanjiNoteService = new KanjiNoteService(anki2Controller);
			List<Note> kanjiNotes = kanjiNoteService.GetKanjiNotes(deckNotes);

			//Act
			List<string> subKanjiIds = anki2Controller.GetSubKanjiIds(kanjiNotes);

			//Assert
			subKanjiIds.Should().BeEmpty();
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, new[] { "1468", "951" })]
		public void Duplicate_sub_kanji_ids_found_is_a_distinct_list(string anki2File, long deckId, string[] expectedSubKanjiIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> deckNotes = anki2Controller.GetDeckNotes(deckId);
			KanjiNoteService kanjiNoteService = new KanjiNoteService(anki2Controller);
			List<Note> kanjiNotes = kanjiNoteService.GetKanjiNotes(deckNotes);

			//Act
			List<string> subKanjiIds = anki2Controller.GetSubKanjiIds(kanjiNotes);

			//Assert
			subKanjiIds.Should().BeEquivalentTo(expectedSubKanjiIds);
		}
	}
}

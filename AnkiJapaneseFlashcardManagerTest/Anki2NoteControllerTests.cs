﻿using AnkiJapaneseFlashcardManager.ApplicationLayer.Services;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManagerTests
{
    public class Anki2NoteControllerTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, new[] { "1474" }, new[] { 1707169522144 })]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160947123, new[] { "1472", "466", "951", "1468" }, new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793 })]
		public void Get_notes_by_kanji_ids(string anki2File, long deckId, string[] kanjiIds, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> notes = anki2Controller.GetDeckNotes(deckId);

			//Redundant? (Filter for "kid:" but then instantly filter again to grab the ids)
			//(Only care about using GetKanjiNotes() when trying to access data that isn't "kid")
			//List<Note> kanjiNotes = anki2Controller.GetKanjiNotes(notes);

			//Act
			List<Note> kanjiNotes = anki2Controller.GetNotesByKanjiIds(notes, kanjiIds);

			//Assert
			kanjiNotes.Select(n => n.Id).Should().BeEquivalentTo(expectedNoteIds);
		}

		[Theory]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", 1707160682667, new[] { "0", "nonExistentId" })]
		public void Invalid_kanji_id_is_empty(string anki2File, long deckId, string[] kanjiIds)
		{
			//Arrange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);
			List<Note> notes = anki2Controller.GetDeckNotes(deckId);

			//Act
			List<Note> kanjiNotes = anki2Controller.GetNotesByKanjiIds(notes, kanjiIds);

			//Assert
			kanjiNotes.Should().BeEmpty();
		}
	}
}

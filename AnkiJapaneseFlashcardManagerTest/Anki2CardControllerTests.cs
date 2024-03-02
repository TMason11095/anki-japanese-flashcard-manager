using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Helpers;
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
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 1, new[] { 1707169497960, 1707169570657, 1707170000793, 1707169983389 })]
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 4, new[] { 1707169983389 })]
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, 5, new[] { 1707169983389 })]
		public void Get_note_ids_with_at_least_the_given_interval(string anki2File, long[] noteIds, int interval, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			Anki2Controller anki2Controller = new Anki2Controller(dbContext);

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
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			Anki2Controller anki2Controller = new Anki2Controller(dbContext);

			//Act
			var noteIdsWithGivenInterval = anki2Controller.GetNoteIdsWithAtLeastInterval(noteIds, interval);

			//Assert
			noteIdsWithGivenInterval.Should().BeEmpty();
		}
	}
}

using AnkiJapaneseFlashcardManager.ApplicationLayer.Services;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManagerTests.ApplicationLayer.Services
{
	public class KanjiCardServiceTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		//Test case: Note ids found
		[InlineData("emptyLearningKanji_1ivl飲12ivl良monthsIvl食欠人newKanji_decks.anki2", new[] { 1548988102030, 1552619440878, 1559353225229, 1559353240186, 1559353264106 }, new[] { 1548988102030, 1552619440878, 1559353225229, 1559353240186 })]
		//Test case: No note ids found
		[InlineData("emptyLearningKanji_0ivl飲1ivl食欠良5ivl人newKanji_decks.anki2", new[] { 1707169522144, 1707169497960, 1707169570657, 1707170000793, 1707169983389 }, new long[0])]
		public void Get_note_ids_with_at_least_the_kanji_interval(string anki2File, long[] noteIds, long[] expectedNoteIds)
		{
			//Arrange
			Anki2Context dbContext = new Anki2Context(_anki2FolderPath + anki2File);
			Anki2Controller anki2Controller = new Anki2Controller(dbContext);
			KanjiCardService kanjiCardService = new KanjiCardService(anki2Controller);

			//Act
			var noteIdsWithKanjiInterval = kanjiCardService.GetNoteIdsWithAtLeastKanjiInterval(noteIds);

			//Assert
			noteIdsWithKanjiInterval.Should().BeEquivalentTo(expectedNoteIds);
		}
	}
}

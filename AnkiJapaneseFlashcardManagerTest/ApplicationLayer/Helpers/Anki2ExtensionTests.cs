using AnkiJapaneseFlashcardManager.ApplicationLayer.Helpers;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.TestHelpers;

namespace Tests.ApplicationLayer.Helpers
{
	public class Anki2ExtensionTests
	{
		[Theory]
		//Test Case: Deck ids found
		[InlineData("empty_kanjiResource_deck.anki2", new[] { 1, 1706982246215, 1706982318565, 1706982351536 })]
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", new[] { 1, 1706982318565, 1706982351536, 1707160682667, 1707160947123 })]
		//Test Case: No deck ids found
		//TODO
		public void Decks_get_ids(string anki2File, long[] expectedDeckIds)
		{
			//Arrange
			Anki2TestHelper helper = new Anki2TestHelper(anki2File);
			List<Deck> decks = helper.GetAllNoTrackingDecks();

			//Act
			var deckIds = decks.GetIds();

			//Assert
			deckIds.Should().BeEquivalentTo(expectedDeckIds);
		}
	}
}

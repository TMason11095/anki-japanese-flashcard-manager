using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManagerTests
{
	public class Anki2DeckControllerTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		[InlineData("empty_kanjiResource_deck.anki2", "KanjiResource")]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", "KanjiResource")]
		[InlineData("empty_random_decks.anki2", "Random")]
		[InlineData("empty_random_decks.anki2", "RandomResource")]
		public void Get_decks_by_description_tag(string anki2File, string deckTagName)//TODO: Refactor to check for expected values
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var taggedDecks = anki2Controller.GetTaggedDecks(deckTagName);

			//Assert
			taggedDecks.Should().NotBeEmpty();
		}

		[Theory]
		[InlineData("empty_kanjiResource_deck.anki2", "NonExistentTag")]
		[InlineData("empty_random_decks.anki2", "KanjiResource")]
		public void No_decks_with_matching_description_tag_is_empty(string anki2File, string deckTagName)
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var taggedDecks = anki2Controller.GetTaggedDecks(deckTagName);

			//Assert
			taggedDecks.Should().BeEmpty();
		}

		[Theory]
		//Test case: Deck ids found
		[InlineData("empty_kanjiResource_deck.anki2", "deck:KanjiResource", new long[] { 1706982246215 })]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", "deck:KanjiResource", new long[] { 1707160947123 })]
		[InlineData("empty_random_decks.anki2", "deck:Random", new long[] { 1706982351536, 1706982318565 })]//Also picks up "deck:RandomResource" because it starts the same
		[InlineData("empty_random_decks.anki2", "deck:RandomResource", new long[] { 1706982318565 })]
		//Test case: No deck ids found
		[InlineData("empty_kanjiResource_deck.anki2", "NonExistentTag", new long[] { })]
		[InlineData("empty_random_decks.anki2", "deck:KanjiResource", new long[] { })]
		public void Get_decks_by_description_containing(string anki2File, string descriptionPart, long[] expectedDeckIds)
		{
			//Arange
			Anki2Controller anki2Controller = new Anki2Controller(_anki2FolderPath + anki2File);

			//Act
			var descDecks = anki2Controller.GetDecksByDescriptionContaining(descriptionPart);

			//Assert
			descDecks.Select(d => d.Id).Should().BeEquivalentTo(expectedDeckIds);
		}
	}
}

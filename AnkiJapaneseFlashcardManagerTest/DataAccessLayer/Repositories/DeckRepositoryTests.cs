﻿using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.TestHelpers;

namespace Tests.DataAccessLayer.Repositories
{
	public class DeckRepositoryTests
	{
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
			DeckRepository deckRepo = new Anki2TestHelper(anki2File).DeckRepository;

			//Act
			var descDecks = deckRepo.GetDecksByDescriptionContaining(descriptionPart);

			//Assert
			descDecks.Select(d => d.Id).Should().BeEquivalentTo(expectedDeckIds);
		}
	}
}

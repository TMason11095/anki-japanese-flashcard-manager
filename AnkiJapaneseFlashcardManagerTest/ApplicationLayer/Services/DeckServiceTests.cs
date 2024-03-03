﻿using AnkiJapaneseFlashcardManager.ApplicationLayer.Services;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.ApplicationLayer.Services
{
	public class DeckServiceTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		//Test case: Deck ids found
		[InlineData("empty_kanjiResource_deck.anki2", "KanjiResource", new long[] { 1706982246215 })]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", "KanjiResource", new long[] { 1707160947123 })]
		[InlineData("empty_random_decks.anki2", "Random", new long[] { 1706982351536, 1706982318565 })]
		[InlineData("empty_random_decks.anki2", "RandomResource", new long[] { 1706982318565 })]
		//Test case: Deck ids not found
		[InlineData("empty_kanjiResource_deck.anki2", "NonExistentTag", new long[0])]
		[InlineData("empty_random_decks.anki2", "KanjiResource", new long[0])]
		public void Get_decks_by_description_tag(string anki2File, string deckTagName, long[] expectedDeckIds)
		{
			//Arange
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));

			//Act
			var taggedDecks = deckService.GetTaggedDecks(deckTagName);

			//Assert
			taggedDecks.Select(d => d.Id).Should().BeEquivalentTo(expectedDeckIds);
		}
	}
}

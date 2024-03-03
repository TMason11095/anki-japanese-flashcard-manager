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
	public class KanjiDeckServiceTests
	{
		//TODO: MOVE TO GLOBAL VARIABLE
		string _anki2FolderPath = "./Resources/Anki2 Files/";

		[Theory]
		//Test case: Deck ids found
		[InlineData("empty_kanjiResource_deck.anki2", new long[] { 1706982246215 })]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", new long[] { 1707160947123 })]
		//Test case: Deck ids not found
		[InlineData("empty_random_decks.anki2", new long[0])]
		public void Get_kanji_resource_decks(string anki2File, long[] expectedDeckIds)
		{
			//Arange
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));
			KanjiDeckService kanjiDeckService = new KanjiDeckService(deckService);

			//Act
			var taggedDecks = kanjiDeckService.GetResourceKanjiDecks();

			//Assert
			taggedDecks.Select(d => d.Id).Should().BeEquivalentTo(expectedDeckIds);
		}

		[Theory]
		//Test case: Deck ids found
		[InlineData("empty_newKanji_deck.anki2", new long[] { 1707160682667 })]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", new long[] { 1707160682667 })]
		//Test case: Deck ids not found
		[InlineData("empty_random_decks.anki2", new long[0])]
		public void Get_new_kanji_decks(string anki2File, long[] expectedDeckIds)
		{
			//Arange
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));
			KanjiDeckService kanjiDeckService = new KanjiDeckService(deckService);

			//Act
			var taggedDecks = kanjiDeckService.GetNewKanjiDecks();

			//Assert
			taggedDecks.Select(d => d.Id).Should().BeEquivalentTo(expectedDeckIds);
		}

		[Theory]
		[InlineData("emptyLearningKanji_飲newKanji_食欠人良resourceKanji_decks.anki2", new long[] { 1707525964862 })]
		public void Get_learning_kanji_decks(string anki2File, long[] expectedDeckIds)
		{
			//Arange
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));
			KanjiDeckService kanjiDeckService = new KanjiDeckService(deckService);

			//Act
			var learningKanjiDecks = kanjiDeckService.GetLearningKanjiDecks();

			//Assert
			learningKanjiDecks.Select(d => d.Id).Should().BeEquivalentTo(expectedDeckIds);
		}

		[Theory]
		[InlineData("empty_random_decks.anki2")]
		public void No_learning_kanji_decks_is_empty(string anki2File)
		{
			//Arange
			Anki2Context anki2Context = new Anki2Context(_anki2FolderPath + anki2File);
			DeckService deckService = new DeckService(new DeckRepository(anki2Context));
			KanjiDeckService kanjiDeckService = new KanjiDeckService(deckService);

			//Act
			var learningKanjiDecks = kanjiDeckService.GetLearningKanjiDecks();

			//Assert
			learningKanjiDecks.Should().BeEmpty();
		}
	}
}

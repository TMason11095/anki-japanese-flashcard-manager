using AnkiJapaneseFlashcardManager.ApplicationLayer.Helpers;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
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

		[Theory]
		//Test Case: Note ids found
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", new[] { 1707169497960, 1707169522144, 1707169570657, 1707169983389, 1707170000793 })]
		//Test Case: No note ids found
		[InlineData("empty_kanjiResource_deck.anki2", new long[0])]
		public void Notes_get_ids(string anki2File, long[] expectedNoteIds)
		{
			//Arrange
			Anki2TestHelper helper = new Anki2TestHelper(anki2File);
			List<Note> notes = helper.GetAllNoTrackingNotes();

			//Act
			var noteIds = notes.GetIds();

			//Assert
			noteIds.Should().BeEquivalentTo(expectedNoteIds);
		}

		[Theory]
		//Test Case: Note ids found
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", new[] { 1707160947123, 1707160682667 }, new[] { 1707169497960, 1707169570657, 1707169983389, 1707170000793, 1707169522144 })]
		//Test Case: No note ids found
		[InlineData("empty_random_decks.anki2", new[] { 1706982318565, 1706982351536 }, new long[0])]
		[InlineData("empty_kanjiResource_newKanji_decks.anki2", new[] { 1706982318565, 1706982351536, 1707160682667, 1707160947123 }, new long[0])]
		//Test Case: No deck ids given
		[InlineData("飲newKanji_食欠人良resourceKanji_decks.anki2", new long[0], new long[0])]
		public void Decks_get_notes(string anki2File, long[] deckIds, long[] expectedNoteIds)
		{
			//Arrange
			Anki2TestHelper helper = new Anki2TestHelper(anki2File);
			IEnumerable<Deck> decks = helper.GetAllNoTrackingDecks()
				.Where(d => deckIds.Contains(d.Id));
			CardRepository cardRepository = helper.CardRepository;

			//Act
			var noteIds = decks.GetNotes(cardRepository);

			//Assert
			noteIds.GetIds().Should().BeEquivalentTo(expectedNoteIds);
		}
	}
}

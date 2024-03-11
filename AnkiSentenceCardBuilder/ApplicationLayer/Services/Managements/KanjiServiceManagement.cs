using AnkiJapaneseFlashcardManager.ApplicationLayer.Helpers;
using AnkiJapaneseFlashcardManager.ApplicationLayer.Interfaces.Services;
using AnkiJapaneseFlashcardManager.ApplicationLayer.Interfaces.Services.Managements;
using AnkiJapaneseFlashcardManager.DomainLayer.Interfaces.Repositories;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services.Managements
{
	public class KanjiServiceManagement : IKanjiServiceManagement
	{
		private readonly IKanjiDeckService _kanjiDeckService;
		private readonly IKanjiNoteService _kanjiNoteService;
		private readonly IKanjiCardService _kanjiCardService;
		private readonly ICardRepository _cardRepository;

		public KanjiServiceManagement(IKanjiDeckService kanjiDeckService, IKanjiNoteService kanjiNoteService, IKanjiCardService kanjicardService, ICardRepository cardRepository)
		{
			_kanjiDeckService = kanjiDeckService;
			_kanjiNoteService = kanjiNoteService;
			_kanjiCardService = kanjicardService;
			_cardRepository = cardRepository;
		}

		public bool MoveNewKanjiToLearningKanji()//Deck and Card(Note)
		{
			//Get new kanji decks
			var newKanjiDecks = _kanjiDeckService.GetNewKanjiDecks();
			//Get the new kanji note ids
			var newKanjiNoteIds = newKanjiDecks.GetNotes(_cardRepository).GetIds();
			//Get the new kanji note ids to be moved (based on the minimum interval)
			var newKanjiNoteIdsToMove = _kanjiCardService.GetNoteIdsWithAtLeastKanjiInterval(newKanjiNoteIds);
			//Get the learning kanji decks
			var learningKanjiDecks = _kanjiDeckService.GetLearningKanjiDecks();
			//Fail if no learning kanji decks found
			if (!learningKanjiDecks.Any()) { return false; }
			//Get the first learning deck id to move the new kanji notes to
			var learningKanjiDeckId = learningKanjiDecks.First().Id;
			//Move the new kanji notes to the learning kanji deck
			return _cardRepository.MoveNotesBetweenDecks(newKanjiNoteIdsToMove, learningKanjiDeckId);
		}

		public bool MoveResourceSubKanjiToNewKanji()//Deck and Card(Note)
		{
			//Get the kanji resource decks
			var kanjiResourceDecks = _kanjiDeckService.GetResourceKanjiDecks();
			//Get the kanji resource notes
			var kanjiResourceNotes = kanjiResourceDecks.GetNotes(_cardRepository).ToList();
			//Get the new kanji decks
			var newKanjiDecks = _kanjiDeckService.GetNewKanjiDecks();
			//Fail if no new kanji decks found
			if (!newKanjiDecks.Any()) { return false; }
			//Get the new kanji notes
			var newKanjiNotes = newKanjiDecks.GetNotes(_cardRepository).ToList();
			//Pull kanji resource notes based on the new kanji sub kanji ids
			var SubKanjiResourceNotes = _kanjiNoteService.PullAllSubKanjiNotesFromNoteList(ref kanjiResourceNotes, newKanjiNotes);
			//Skip if no new kanji sub kanji notes to move
			if (!SubKanjiResourceNotes.Any()) { return true; }
			//Get the sub kanji resource note ids
			var subKanjiResourceNoteIdsToMove = SubKanjiResourceNotes.GetIds();
			//Get the first new kanji deck id to move the resource kanji notes to
			var newKanjiDeckId = newKanjiDecks.First().Id;
			//Move the resource kanji notes to the new kanji deck
			return _cardRepository.MoveNotesBetweenDecks(subKanjiResourceNoteIdsToMove, newKanjiDeckId);
		}
	}
}

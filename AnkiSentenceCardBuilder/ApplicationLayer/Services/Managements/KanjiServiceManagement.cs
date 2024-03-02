using AnkiSentenceCardBuilder.Controllers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services.Managements
{
	public class KanjiServiceManagement
	{
		private readonly Anki2Controller _anki2Controller;
		private readonly KanjiDeckService _kanjiDeckService;
		private readonly KanjiNoteService _kanjiNoteService;
		private readonly KanjiCardService _kanjiCardService;

		public KanjiServiceManagement(Anki2Controller anki2Controller, KanjiDeckService kanjiDeckService, KanjiCardService kanjicardService)
		{
			_anki2Controller = anki2Controller;
			_kanjiDeckService = kanjiDeckService;
			_kanjiNoteService = new KanjiNoteService(anki2Controller);
			_kanjiCardService = kanjicardService;
		}

		public bool MoveNewKanjiToLearningKanji()//Deck and Card(Note)
		{
			//Get new kanji decks
			var newKanjiDecks = _kanjiDeckService.GetNewKanjiDecks();
			//Get the new kanji note ids
			var newKanjiNoteIds = newKanjiDecks.SelectMany(d => _anki2Controller.GetDeckNotes(d.Id)).Select(n => n.Id).ToList();
			//Get the new kanji note ids to be moved (based on the minimum interval)
			var newKanjiNoteIdsToMove = _kanjiCardService.GetNoteIdsWithAtLeastKanjiInterval(newKanjiNoteIds);
			//Get the learning kanji decks
			var learningKanjiDecks = _kanjiDeckService.GetLearningKanjiDecks();
			//Fail if no learning kanji decks found
			if (!learningKanjiDecks.Any()) { return false; }
			//Get the first learning deck id to move the new kanji notes to
			var learningKanjiDeckId = learningKanjiDecks.First().Id;
			//Move the new kanji notes to the learning kanji deck
			return _anki2Controller.MoveNotesBetweenDecks(newKanjiNoteIdsToMove, learningKanjiDeckId);
		}

		public bool MoveResourceSubKanjiToNewKanji()//Deck and Card(Note)
		{
			//Get the kanji resource decks
			var kanjiResourceDecks = _kanjiDeckService.GetResourceKanjiDecks();
			//Get the kanji resource notes
			var kanjiResourceNotes = kanjiResourceDecks.SelectMany(d => _anki2Controller.GetDeckNotes(d.Id)).ToList();
			//Get the new kanji decks
			var newKanjiDecks = _kanjiDeckService.GetNewKanjiDecks();
			//Fail if no new kanji decks found
			if (!newKanjiDecks.Any()) { return false; }
			//Get the new kanji notes
			var newKanjiNotes = newKanjiDecks.SelectMany(d => _anki2Controller.GetDeckNotes(d.Id)).ToList();
			//Pull kanji resource notes based on the new kanji sub kanji ids
			var SubKanjiResourceNotes = _kanjiNoteService.PullAllSubKanjiNotesFromNoteList(ref kanjiResourceNotes, newKanjiNotes);
			//Skip if no new kanji sub kanji notes to move
			if (!SubKanjiResourceNotes.Any()) { return true; }
			//Get the sub kanji resource note ids
			var subKanjiResourceNoteIdsToMove = SubKanjiResourceNotes.Select(n => n.Id).ToList();
			//Get the first new kanji deck id to move the resource kanji notes to
			var newKanjiDeckId = newKanjiDecks.First().Id;
			//Move the resource kanji notes to the new kanji deck
			return _anki2Controller.MoveNotesBetweenDecks(subKanjiResourceNoteIdsToMove, newKanjiDeckId);
		}
	}
}

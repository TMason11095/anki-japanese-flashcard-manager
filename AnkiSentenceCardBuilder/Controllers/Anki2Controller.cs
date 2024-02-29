﻿using AnkiJapaneseFlashcardManager.Config;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiSentenceCardBuilder.Controllers
{
    public class Anki2Controller : IDisposable
	{
        private readonly Anki2Context _context;

        public Anki2Controller(Anki2Context context)
        {
            _context = context;
        }

        public Anki2Controller(string dbPath) : this(new Anki2Context(dbPath))
        {
        }

		public void Dispose()
		{
			SqliteConnection.ClearPool((SqliteConnection) _context.Database.GetDbConnection());
			_context.Dispose();
		}

		public List<T> GetTable<T>() where T : class
        {
            return _context.Set<T>().AsNoTracking().ToList();
        }

        private static string DecodeBlob(byte[] blob)
        {
            return System.Text.Encoding.UTF8.GetString(blob);
		}

		public IEnumerable<Deck> GetDecksByDescriptionContaining(string descriptionPart)
		{
			//Get all the decks
			var decks = _context.Decks;
			//Remap to decode the description field (Kind) (Convert to List as the following .Where() tries calling DecodeBlob() and fails if you don't)
			var deckDescs = decks.Select(d => new
			{
				deck = d,
				description = DecodeBlob(d.Kind)
			}).ToList();
			//Filter to find the decks with the tag in its description
			var DecksContaining = deckDescs
				.Where(d => d.description.Contains(descriptionPart, StringComparison.OrdinalIgnoreCase))
				.Select(d => d.deck);
			//Return
			return DecksContaining;
		}

		//public Deck GetDeckById(long deckId)//TODO
		//{
		//	return null;
		//}

		public List<Note> GetDeckNotes(long deckId)//Card(Note)
		{
			//Return the notes from unique card entries with the given deck id
			return _context.Cards
					.Where(c => c.DeckId == deckId) //Grab cards with matching deck id
					.Select(c => c.Note) //Grab the notes
					.Distinct() //Filter out duplicate entries
					.ToList();
		}

		//public Note GetNoteById(long noteId)//TODO
		//{
		//	return null;
		//}

		//public List<long> GetSubKanjiIds(Note kanjiNote)//TODO
		//{
		//	return null;
		//}

		public List<Note> GetNotesByKanjiIds(IEnumerable<Note> kanjiNotes, IEnumerable<string> kanjiIds)//Note
		{
			//Get kanji id tag
			string kanjiIdTag = AnkiBindingConfig.Bindings.NoteTags.KanjiId;
			//Return the kanji notes with matching ids
			return kanjiNotes.Where(n => GetIdsFromTagList(n.TagsList, kanjiIdTag).Exists(id => kanjiIds.Contains(id))).ToList();
		}

		public List<string> GetIdsFromTagList(List<string> tagList, string tag)//Note?
		{
			return tagList.Where(t => t.StartsWith(tag))
						.Select(t => t.Substring(tag.Length))
						.ToList();
		}

		public bool MoveNotesBetweenDecks(IEnumerable<long> noteIds, long newDeckId)//Card(Note)
		{
			//Grab all the cards (Note/Deck junction table) with the given note ids
			var existingCards = _context.Cards.Where(c => noteIds.Contains(c.NoteId));
			//Update the deck id for each card
			foreach (var card in existingCards)
			{
				card.DeckId = newDeckId;
			}
			//Save the changes
			_context.SaveChanges();
			//Return success
			return true;
		}

		public IEnumerable<long> GetNoteIdsWithAtLeastInterval(IEnumerable<long> noteIds, int interval)//Card(Note)
		{
			return _context.Cards
						.Where(c => noteIds.Contains(c.NoteId))//Grab cards with matching note ids
						.Where(c => c.Interval >= interval)//Filter cards with matching intervals
						.Select(c => c.NoteId)//Grab the note ids
						.ToList();//Return the list
		}

		/// <summary>
		/// Get the minimum interval for each note in a list of note ids
		/// </summary>
		/// <param name="noteIds"></param>
		/// <returns>
		/// Dictionary of note ids to their minimum interval (if a card type that has duplicate notes)
		/// </returns>
		//public Dictionary<long, int> GetMinIntervalForEachNote(IEnumerable<long> noteIds)
		//{
		//	return null;
		//}

		public IEnumerable<long> GetNoteIdsWithAtLeastKanjiInterval(IEnumerable<long> noteIds)//Card(Note)
		{
			//Get the minimum interval for moving newKanji into learningKanji
			int newKanjiInterval = AnkiBindingConfig.Bindings.NoteIntervalLimits.MoveFromNewKanji;
			//Return the note ids with the minimum interval
			return GetNoteIdsWithAtLeastInterval(noteIds, newKanjiInterval);
		}

		
	}
}

using AnkiJapaneseFlashcardManager.ApplicationLayer.Services;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Helpers;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestHelpers
{
	public class Anki2TestHelper : IDisposable
	{
		private const string Anki2FolderPath = "./Resources/Anki2 Files/";
		private string _anki2File;
		private string _anki2FilePath;
		private string _anki2TempFilePath;
		private bool _useEditableCopy;
		private string _currentlyUsedFilePath;

		public Anki2Context Anki2Context { get; private set; }

		private DeckRepository _deckRepository;
		public DeckRepository DeckRepository
		{
			get { return SingletonObject(ref _deckRepository, () => new DeckRepository(this.Anki2Context)); }
			private set { _deckRepository = value; }
		}
		
		private CardRepository _cardRepository;
		public CardRepository CardRepository
		{
			get { return SingletonObject(ref _cardRepository, () => new CardRepository(this.Anki2Context)); }
			private set { _cardRepository = value; }
		}

		private DeckService _deckService;
		public DeckService DeckService
		{
			get { return SingletonObject(ref _deckService, () => new DeckService(this.DeckRepository)); }
			private set { _deckService = value; }
		}

		private KanjiDeckService _kanjiDeckService;
		public KanjiDeckService KanjiDeckService
		{
			get { return SingletonObject(ref _kanjiDeckService, () => new KanjiDeckService(this.DeckService)); }
			private set { _kanjiDeckService = value; }
		}

		private KanjiNoteService _kanjiNoteService;
		public KanjiNoteService KanjiNoteService
		{
			get { return SingletonObject(ref _kanjiNoteService, () => new KanjiNoteService()); }
			private set { _kanjiNoteService = value; }
		}

		private T SingletonObject<T>(ref T singletonObject, Func<T> constructor)
			where T : class
		{
			if (singletonObject is null)
			{
				singletonObject = constructor();
			}
			//Return
			return singletonObject;
		}

		public Anki2TestHelper(string anki2File, bool createTempCopy = false)
		{
			//Setup the file path
			_anki2File = anki2File;
			_anki2FilePath = Anki2FolderPath + _anki2File;
			_currentlyUsedFilePath = _anki2FilePath;

			//Create the editable copy
			_useEditableCopy = createTempCopy;
			if (_useEditableCopy)
			{
				//Create a temp copy of the anki2 file
				_anki2TempFilePath = $"{Anki2FolderPath}temp_{Guid.NewGuid()}.anki2";
				File.Copy(_anki2FilePath, _anki2TempFilePath, true);
				//Set the current file path
				_currentlyUsedFilePath = _anki2TempFilePath;
			}

			//Setup the DbContext
			Anki2Context = new Anki2Context(_currentlyUsedFilePath);
		}

		public void Dispose()
		{
			//Clean up the temp file if needed
			if (_useEditableCopy)
			{
				DbContextHelper.ClearSqlitePool(Anki2Context);
				File.Delete(_anki2TempFilePath);
			}
		}
	}
}

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

		private Anki2Context _anki2Context;

		private DeckRepository _deckRepository;
		private CardRepository _cardRepository;

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
			_anki2Context = new Anki2Context(_currentlyUsedFilePath);
		}

		public void Dispose()
		{
			//Clean up the temp file if needed
			if (_useEditableCopy)
			{
				DbContextHelper.ClearSqlitePool(_anki2Context);
				File.Delete(_anki2TempFilePath);
			}
		}

		public DeckRepository GetDeckRepository()
		{
			//Create new instance if 1st time
			if (_deckRepository is null)
			{
				_deckRepository = new DeckRepository(_anki2Context);
			}
			//Return
			return _deckRepository;
		}

		public CardRepository GetCardRepository()
		{
			//Create new instance if 1st time
			if (_cardRepository is null)
			{
				_cardRepository = new CardRepository(_anki2Context);
			}
			//Return
			return _cardRepository;
		}
	}
}

using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.TestHelpers
{
	public class Anki2TestHelper
	{
		private const string Anki2FolderPath = "./Resources/Anki2 Files/";
		private string _anki2File;
		private string _anki2FilePath;
		private Anki2Context _anki2Context;
		private DeckRepository _deckRepository;

		public Anki2TestHelper(string anki2File)//, bool createEditableCopy = false)
		{
			//Setup the file path
			_anki2File = anki2File;
			_anki2FilePath = Anki2FolderPath + _anki2File;

			////Create the editable copy
			//if (createEditableCopy)
			//{

			//}

			//Setup the DbContext
			_anki2Context = new Anki2Context(_anki2FilePath);
		}

		public DeckRepository GetDeckRepository()
		{
			if (_deckRepository is null)
			{
				_deckRepository = new DeckRepository(_anki2Context);
			}
			//Return
			return _deckRepository;
		}
	}
}

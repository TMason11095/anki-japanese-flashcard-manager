using AnkiJapaneseFlashcardManager.Config;
using AnkiSentenceCardBuilder.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.ApplicationLayer.Services
{
	public class KanjiCardService
	{
		private readonly Anki2Controller _controller;

		public KanjiCardService(Anki2Controller controller)
		{
			_controller = controller;
		}

		public IEnumerable<long> GetNoteIdsWithAtLeastKanjiInterval(IEnumerable<long> noteIds)//Card(Note)
		{
			//Get the minimum interval for moving newKanji into learningKanji
			int newKanjiInterval = AnkiBindingConfig.Bindings.NoteIntervalLimits.MoveFromNewKanji;
			//Return the note ids with the minimum interval
			return _controller.GetNoteIdsWithAtLeastInterval(noteIds, newKanjiInterval);
		}
	}
}

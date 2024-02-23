using AnkiSentenceCardBuilder.Controllers;
using AnkiSentenceCardBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiJapaneseFlashcardManager.Controllers
{
	public class Anki2DeckController : Anki2Controller
	{
		public Anki2DeckController(Anki2Context context) : base(context)
		{
		}
	}
}

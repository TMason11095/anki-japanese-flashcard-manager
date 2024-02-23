using System.Net;
using System.Runtime.InteropServices;
using AnkiJapaneseFlashcardManager.Config;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiSentenceCardBuilder.Controllers;
using Grpc.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Deck = AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts.Deck;

namespace AnkiSentenceCardBuilder.Services
{
    public class LocalTestingFunction
	{
		private readonly ILogger _logger;

		public LocalTestingFunction(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<LocalTestingFunction>();
		}

		//Used for local testing of main functions (instead of re-adding files to the blob each time)
		[Function("LocalTestingFunction")]
		public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
		{
			//Log
			_logger.LogInformation("C# HTTP trigger function processed a test run request.");

			//Set the temp file location to use
			string tempDbPath = @"C:\Users\Tyler\AppData\Roaming\Anki2Sentence\collection.anki2";

			//Setup db controller
			Anki2Controller anki2Controller = new Anki2Controller(tempDbPath);

			//List<Deck> decks = anki2Controller.GetTable<Deck>();

			//Find the decks with the kanji resource binding
				List<Deck> resourceKanjiDecks = anki2Controller.GetResourceKanjiDecks();
			//Find the decks with the new kanji binding
			List<Deck> newKanjiDecks = anki2Controller.GetNewKanjiDecks();
			//Find the decks with the learning kanji binding
			List<Deck> learningKanjiDecks = anki2Controller.GetLearningKanjiDecks();

			//Get notes from the kanji resource decks
				List<Note> resourceKanjiNotes = resourceKanjiDecks.SelectMany(d => anki2Controller.GetDeckNotes(d.Id)).ToList();
			//Get notes from the new kanji decks
			List<Note> newKanjiNotes = newKanjiDecks.SelectMany(d => anki2Controller.GetDeckNotes(d.Id)).ToList();

			//Pull kanji notes from the resource kanji decks based on the new kanji notes' sub kanji ids
			List<Note> newKanjiSubKanjiNotes = anki2Controller.PullAllSubKanjiNotesFromNoteList(ref resourceKanjiNotes, newKanjiNotes);

			//Update the DB to move the pulled notes from the resource kanji decks to the new kanji decks
			bool movedFromResourceToNewKanji = anki2Controller.MoveNotesBetweenDecks(newKanjiSubKanjiNotes.Select(n => n.Id), newKanjiDecks.Select(d => d.Id).First());

			//Update to pull sub kanji notes from the kanji resource decks into the new kanji deck based on the existing new kanji notes

			//Update to pull valid new kanji notes into the learning kanji deck
			bool movedFromNewKanjiToLearningKanjiDeck = anki2Controller.MoveNewKanjiToLearningKanji();

			anki2Controller.Dispose();

			//Exit
			var response = req.CreateResponse(HttpStatusCode.OK);
			return response;
		}
	}
}

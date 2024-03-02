using System.IO;
using System.Threading.Tasks;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Contexts;
using AnkiJapaneseFlashcardManager.DataAccessLayer.Helpers;
using AnkiJapaneseFlashcardManager.DomainLayer.Entities;
using AnkiSentenceCardBuilder.Controllers;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AnkiSentenceCardBuilder.Services
{
    public class UpdateAnki2
    {
        private readonly ILogger<UpdateAnki2> _logger;

        public UpdateAnki2(ILogger<UpdateAnki2> logger)
        {
            _logger = logger;
        }

        [Function(nameof(UpdateAnki2))]
        public async Task Run([BlobTrigger("anki2-file/{name}.anki2", Connection = "BlobConnectionString")] BlockBlobClient blobClient, string name)
        {
            //Copy the blob to a local temp folder
            string tempDbPath = await CopyBlobToTempFolder(blobClient, folderName: "Anki2Sentence", fileName: $"{name}.anki2");

			//Setup db controller
			Anki2Context dbContext = new Anki2Context(tempDbPath);
			Anki2Controller anki2Controller = new Anki2Controller(dbContext);

            //Get the decks
            //var decks = anki2Controller.GetTable<Deck>();

			//Cleanup
			DbContextHelper.ClearSqlitePool(dbContext);


			//using var blobStreamReader = new StreamReader(stream);
			//var content = await blobStreamReader.ReadToEndAsync();
			_logger.LogInformation($"C# Blob trigger function Processed blob\n Name: {name} Temp: {tempDbPath}");
        }

        //Returns full path where the blob was copied to
        public async Task<string> CopyBlobToTempFolder(BlockBlobClient blobClient, string folderName, string fileName)
        {
            //Create the directory path
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), folderName);
            //Add any missing directories
            Directory.CreateDirectory(directoryPath);
            //Add file name for the full path
            string fullPath = Path.Combine(directoryPath, fileName);
            //Copy blob to the temp path
            await blobClient.DownloadToAsync(fullPath);
            //Return the path
            return fullPath;
        }
    }
}

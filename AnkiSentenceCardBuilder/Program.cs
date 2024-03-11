using AnkiJapaneseFlashcardManager.DataAccessLayer.Repositories;
using AnkiJapaneseFlashcardManager.DomainLayer.Interfaces.Repositories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
		//Dependency injection
		services.AddScoped<ICardRepository, CardRepository>();
		services.AddScoped<IDeckRepository, DeckRepository>();
	})
    .Build();

host.Run();

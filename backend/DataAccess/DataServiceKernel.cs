using LanguageForge.Api.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver.Linq;

namespace LanguageForge.Api;

public static class DataServiceKernel
{
    public static void Setup(IServiceCollection services)
    {
        BsonConfiguration.Setup();
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var mongoSettings = MongoClientSettings.FromConnectionString(
                configuration.GetValue<string>("Mongo:ConnectionString"));
            mongoSettings.LinqProvider = LinqProvider.V3;
            mongoSettings.LoggingSettings = new LoggingSettings(provider.GetRequiredService<ILoggerFactory>());
            return mongoSettings;
        });
        services.AddSingleton(provider => new MongoClient(provider.GetRequiredService<MongoClientSettings>()));

        services.AddSingleton<SystemDbContext>();
        services.AddSingleton<ProjectDbContext>();
    }
}

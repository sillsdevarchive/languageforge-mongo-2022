using EphemeralMongo;
using LanguageForge.Api;
using LanguageForge.WebApi;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;

namespace LanguageForge.UnitTests.Fixtures;

public class IocFixture : IDisposable
{
    public IMongoRunner MongoRunner { get; }
    public ServiceProvider ServiceProvider { get; }

    public IocFixture()
    {
        var services = new ServiceCollection();
        DataServiceKernel.Setup(services);
        WebApiKernel.Setup(services);
        MongoRunner = SetupMongoRunner();
        var clientSettings = MongoClientSettings.FromConnectionString(MongoRunner.ConnectionString);
        services.RemoveAll(typeof(MongoClientSettings));
        services.AddSingleton(clientSettings);
        ServiceProvider = services.BuildServiceProvider(true);
    }

    private IMongoRunner SetupMongoRunner()
    {
        var runner = EphemeralMongo.MongoRunner.Run(new MongoRunnerOptions
        {
            StandardErrorLogger = text =>
            {
                //note does not work, used for breakpoints. TODO fix
                Console.Write(text);
            }
        });
        var testDataPath = Path.GetFullPath("TestDatabase");
        runner.Import(SystemDbContext.SystemDbName, "projects", Path.Combine(testDataPath, "projects.json"));
        runner.Import(SystemDbContext.SystemDbName, "users", Path.Combine(testDataPath, "users.json"));
        runner.Import(SystemDbContext.SystemDbName, @"userrelation", Path.Combine(testDataPath, @"userrelation.json"));
        return runner;
    }

    public void Dispose()
    {
        MongoRunner.Dispose();
        ServiceProvider.Dispose();
    }
}

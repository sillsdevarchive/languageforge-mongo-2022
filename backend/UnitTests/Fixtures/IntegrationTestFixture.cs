using EphemeralMongo;
using LanguageForge.Api;
using LanguageForge.WebApi;
using LanguageForge.WebApi.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting.Internal;
using MongoDB.Driver;

namespace LanguageForge.UnitTests.Fixtures;

public class IntegrationTestFixture : IDisposable
{
    public IMongoRunner MongoRunner { get; }
    public ServiceProvider Services { get; }

    public IntegrationTestFixture()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationManager()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();
        var environment = new HostingEnvironment();
        services.AddSingleton<IConfiguration>(configuration);
        DataServiceKernel.Setup(services);
        WebApiKernel.Setup(services);
        AuthSetup.SetupLfAuth(services, configuration, environment);
        MongoRunner = SetupMongoRunner();
        services.RemoveAll(typeof(MongoClientSettings));
        services.Replace(ServiceDescriptor.Singleton(provider => DataServiceKernel.BuildMongoClientSettings(MongoRunner.ConnectionString, provider)));
        Services = services.BuildServiceProvider(true);
    }

    private IMongoRunner SetupMongoRunner()
    {
        var runner = EphemeralMongo.MongoRunner.Run(new MongoRunnerOptions
        {
            StandardErrorLogger = Console.Write
        });
        var testDataPath = Path.GetFullPath("TestDatabase");
        runner.Import(SystemDbContext.SystemDbName, "projects", Path.Combine(testDataPath, "projects.json"));
        runner.Import(SystemDbContext.SystemDbName, "users", Path.Combine(testDataPath, "users.json"));
        runner.Import(SystemDbContext.SystemDbName, "userrelation", Path.Combine(testDataPath, "userrelation.json"));
        return runner;
    }

    public void Dispose()
    {
        MongoRunner.Dispose();
        Services.Dispose();
    }
}

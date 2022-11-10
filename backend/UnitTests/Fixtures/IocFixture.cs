using EphemeralMongo;
using LanguageForge.Api;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace UnitTests.Fixtures;

public class IocFixture : IDisposable
{
    private readonly IMongoRunner _mongoRunner;
    public ServiceProvider ServiceProvider { get; }

    public IocFixture()
    {
        var services = new ServiceCollection();
        DataServiceKernel.Setup(services);
        WebApiKernel.Setup(services);
        _mongoRunner = GetMongoRunner();
        var clientSettings = MongoClientSettings.FromConnectionString(_mongoRunner.ConnectionString);
        services.AddSingleton(clientSettings);
        ServiceProvider = services.BuildServiceProvider(true);
    }

    private IMongoRunner GetMongoRunner()
    {
        return MongoRunner.Run();
    }

    public void Dispose()
    {
        _mongoRunner.Dispose();
        ServiceProvider.Dispose();
    }
}
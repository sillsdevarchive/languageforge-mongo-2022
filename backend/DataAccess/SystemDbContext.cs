using LanguageForge.Api.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LanguageForge.Api;

public class SystemDbContext
{
    public const string SystemDbName = "scriptureforge";
    private readonly IMongoDatabase _mongoDatabase;

    public SystemDbContext(MongoClientSettings mongoClientSettings)
    {
        var mongoClient = new MongoClient(mongoClientSettings);
        _mongoDatabase = mongoClient.GetDatabase(SystemDbName);
        Users = _mongoDatabase.GetCollection<User>("Users");
        Projects = _mongoDatabase.GetCollection<Project>("projects");
    }

    public IMongoCollection<User> Users { get; }
    public IMongoCollection<Project> Projects { get; }
}

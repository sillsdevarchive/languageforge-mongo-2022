using LanguageForge.Api.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LanguageForge.Api;

public class SystemDbContext
{
    private readonly IMongoDatabase _mongoDatabase;

    public SystemDbContext(IOptions<MongoClientSettings> mongoClientSettings)
    {
        var mongoClient = new MongoClient(mongoClientSettings.Value);
        _mongoDatabase = mongoClient.GetDatabase("scriptureforge");
        Users = _mongoDatabase.GetCollection<User>("Users");
        Projects = _mongoDatabase.GetCollection<Project>("projects");
    }

    public IMongoCollection<User> Users { get; }
    public IMongoCollection<Project> Projects { get; }
}

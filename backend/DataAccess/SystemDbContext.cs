using LanguageForge.Api.Entities;
using MongoDB.Driver;

namespace LanguageForge.Api;

public class SystemDbContext
{
    public const string SystemDbName = "scriptureforge";
    private readonly IMongoDatabase _mongoDatabase;

    public SystemDbContext(MongoClient mongoClient)
    {
        _mongoDatabase = mongoClient.GetDatabase(SystemDbName);
        Users = _mongoDatabase.GetCollection<User>("users");
        Projects = _mongoDatabase.GetCollection<Project>("projects");
    }

    public IMongoCollection<User> Users { get; }
    public IMongoCollection<Project> Projects { get; }
}

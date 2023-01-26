using LanguageForge.Api.Entities;
using MongoDB.Driver;

namespace LanguageForge.Api;

public class ProjectDbContext
{
    private readonly MongoClient _mongoClient;
    private readonly ILfProjectContext _projectContext;

    private string ProjectCode
    {
        get
        {
            if (_projectContext.ProjectCode == null)
            {
                throw new InvalidOperationException($"{nameof(ILfProjectContext.ProjectCode)} can only be used in the context of individual projects.");
            }
            return _projectContext.ProjectCode;
        }
    }

    public ProjectDbContext(MongoClient mongoClient, ILfProjectContext projectContext)
    {
        _mongoClient = mongoClient;
        _projectContext = projectContext;
    }

    private IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _mongoClient.GetDatabase($"sf_{ProjectCode}").GetCollection<T>(collectionName);
    }

    public IMongoCollection<Entry> Entries()
    {
        return GetCollection<Entry>("lexicon");
    }

    public Task<List<Option>> PartsOfSpeech()
    {
        return GetCollection<OptionList>("optionlists")
            .Find(list => list.Code == "grammatical-info")
            .Project(list => list.Items)
            .SingleOrDefaultAsync();
    }
}

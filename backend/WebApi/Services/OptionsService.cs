using LanguageForge.Api;
using MongoDB.Driver;

namespace LanguageForge.WebApi.Services;

public class OptionsService
{
    private readonly ProjectDbContext _projectDbContext;

    public OptionsService(ProjectDbContext projectDbContext)
    {
        _projectDbContext = projectDbContext;
    }

    public async Task<List<string>> FindPartsOfSpeechKeys(string partOfSpeech)
    {
        var partsOfSpeech = await _projectDbContext.PartsOfSpeech();

        return partsOfSpeech
            .Where(o => o.Value.ContainsLf(partOfSpeech))
            .Select(o => o.Key)
            .ToList();
    }
}

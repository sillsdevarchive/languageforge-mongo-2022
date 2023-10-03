using LanguageForge.Api;
using LanguageForge.Api.Entities;
using LanguageForge.WebApi.Dtos;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace LanguageForge.WebApi.Services;

public class EntryService
{
    private readonly ProjectDbContext _projectDbContext;
    private readonly OptionsService _optionsService;

    public EntryService(ProjectDbContext projectDbContext, OptionsService optionsService)
    {
        _projectDbContext = projectDbContext;
        _optionsService = optionsService;
    }

    public async Task<List<EntryDto>> FindEntries(string? filter, string? inputSystem, string? partOfSpeech, string? semanticDomain, int? skip, int? take)
    {
        var partsOfSpeech = await FindPartsOfSpeechKeys(partOfSpeech);

        var entries = await _projectDbContext.Entries()
            .Find(FilterDefinition<Entry>.Empty)
            .ToListAsync();

        var filteredEntries = entries.Where(e =>
            e.MatchesFilterAndInputSystem(filter, inputSystem)
            && e.MatchesPartsOfSpeech(partsOfSpeech)
            && e.MatchesSemanticDomain(semanticDomain))
            .Skip(skip ?? 0)
            .Take(take ?? int.MaxValue);

        return filteredEntries.Select(e => new EntryDto
        {
            Lexeme = e.Lexeme,
            CitationForm = e.CitationForm,
            Pronunciation = e.Pronunciation,
            CVPattern = e.CVPattern,
            Tone = e.Tone,
            Location = e.Location?.Value,
            Etymology = e.Etymology,
            EtymologyGloss = e.EtymologyGloss,
            EtymologyComment = e.EtymologyComment,
            EtymologySource = e.EtymologySource,
            Note = e.Note,
            LiteralMeaning = e.LiteralMeaning,
            Bibliography = e.Bibliography,
            Restrictions = e.Restrictions,
            SummaryDefinition = e.SummaryDefinition,
            Senses = e.Senses?.Select(s => new SenseDto
            {
                Guid = s.Guid,
                Gloss = s.Gloss,
                Definition = s.Definition,
                Pictures = s.Pictures?.Select(p => new FileDto { Guid = p.Guid, FileName = p.FileName }).ToList(),
                PartOfSpeech = s.PartOfSpeech?.Value,
                SemanticDomain = s.SemanticDomain?.Values,
                ScientificName = s.ScientificName,
                AnthropologyNote = s.AnthropologyNote,
                Bibliography = s.Bibliography,
                DiscourseNote = s.DiscourseNote,
                EncyclopedicNote = s.EncyclopedicNote,
                GeneralNote = s.GeneralNote,
                GrammarNote = s.GrammarNote,
                PhonologyNote = s.PhonologyNote,
                Restrictions = s.Restrictions,
                SemanticsNote = s.SemanticsNote,
                SociolinguisticsNote = s.SociolinguisticsNote,
                Source = s.Source,
                SenseType = s.SenseType?.Value,
                Status = s.Status?.Value,
                Examples = s.Examples?.Select(ex => new ExampleDto
                {
                    Guid = ex.Guid,
                    Sentence = ex.Sentence,
                    Translation = ex.Translation,
                    Reference = ex.Reference,
                }).ToList(),
            }).ToList()
        }).ToList();
    }

    private async Task<List<string>?> FindPartsOfSpeechKeys(string? partOfSpeech)
    {
        if (string.IsNullOrWhiteSpace(partOfSpeech))
        {
            return null;
        }

        var partsOfSpeech = await _optionsService.FindPartsOfSpeechKeys(partOfSpeech);
        if (partsOfSpeech.IsNullOrEmpty())
        {
            throw new ArgumentOutOfRangeException(nameof(partOfSpeech), $"No parts of speech match '{partOfSpeech}'.");
        }
        return partsOfSpeech;
    }
}

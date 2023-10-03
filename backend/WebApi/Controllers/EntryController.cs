using LanguageForge.WebApi.Dtos;
using LanguageForge.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using static LanguageForge.WebApi.Controllers.PathConstants;

namespace LanguageForge.WebApi.Controllers;

[ApiController]
[Route($"api/[controller]/{{{ProjectCode}}}")]
public class EntryController : ControllerBase
{
    private readonly EntryService _entryService;

    public EntryController(EntryService entryService)
    {
        _entryService = entryService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EntryDto>>> GetEntries(string? filter, string? inputSystem, string? partOfSpeech, string? semanticDomain, int? skip, int? take)
    {
        return await _entryService.FindEntries(filter, inputSystem, partOfSpeech, semanticDomain, skip, take);
    }
}

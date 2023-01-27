using LanguageForge.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using static LanguageForge.WebApi.Controllers.PathConstants;

namespace LanguageForge.WebApi.Controllers;

[ApiController]
[Route($"api/[controller]/{{{ProjectCode}}}")]
public class EntryController : ControllerBase
{
    [HttpGet]
    public List<EntryDto> GetEntries()
    {
        return new() {
            new() {
                Lexeme = new [] {
                    new InputSystemValueDto("en", "Dude"),
                    new InputSystemValueDto("de", "Kerl"),
                },
                CitationForm = new [] {
                    new InputSystemValueDto("en", "Mr"),
                    new InputSystemValueDto("de", "Herr"),
                },
                CVPattern = new [] {
                    new InputSystemValueDto("de", "pattern"),
                },
                Sense = new [] {
                    new SenseDto() {
                        Gloss = new [] {
                            new InputSystemValueDto("en", "Dude"),
                            new InputSystemValueDto("de", "Kerl"),
                        },
                        Definition = new [] {
                            new InputSystemValueDto("en", "A cool preson"),
                            new InputSystemValueDto("de", "Eine beliebte Person"),
                        },
                        PartOfSpeech = PartOfSpeechDto.Noun,
                        SemanticDomain = new [] {
                            "2.6.3.4 Labour and birth pains",
                            "1.1.3.5 Storm",
                        },
                    }
                },
            }
        };
    }
}

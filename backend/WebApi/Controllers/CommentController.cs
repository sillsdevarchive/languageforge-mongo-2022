using LanguageForge.WebApi.Validation;
using Microsoft.AspNetCore.Mvc;

namespace LanguageForge.WebApi.Controllers;

[ApiController]
[Route($"api/[controller]/{{entryId}}")]
[RequireProjectCode]
public class CommentController : ControllerBase
{
    [HttpGet("{fieldName}/{inputSystem}")]

    public void GetEntryFieldComment(string fieldName, string inputSystem)
    {

    }

    [HttpGet("{senseId}/{fieldName}/{inputSystem}")]

    public void GetSenseFieldComment(Guid senseId, string fieldName, string inputSystem)
    {

    }
}

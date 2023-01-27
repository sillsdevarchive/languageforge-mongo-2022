using Microsoft.AspNetCore.Mvc;
using static LanguageForge.WebApi.Controllers.PathConstants;

namespace LanguageForge.WebApi.Controllers;

[ApiController]
[Route($"api/[controller]/{{{ProjectCode}}}/{{entryId}}")]
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

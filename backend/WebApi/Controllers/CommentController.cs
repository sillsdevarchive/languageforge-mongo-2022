using Microsoft.AspNetCore.Mvc;

namespace LanguageForge.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/{projectCode}/{entryId}")]
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

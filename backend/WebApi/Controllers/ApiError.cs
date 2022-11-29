namespace LanguageForge.WebApi.Controllers;

public class ApiError
{
    public string Message { get; init; }

    public ApiError(Exception exception)
    {
        Message = exception.ToString();
    }
}

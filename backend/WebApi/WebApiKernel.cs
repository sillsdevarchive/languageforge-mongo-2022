using LanguageForge.Api.Services;

namespace LanguageForge.Api;

public static class WebApiKernel
{
    public static void Setup(IServiceCollection services)
    {
        services.AddSingleton<ProjectService>();
    }
}

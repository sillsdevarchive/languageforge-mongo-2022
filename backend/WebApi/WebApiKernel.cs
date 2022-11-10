using LanguageForge.WebApi.Services;

namespace LanguageForge.WebApi;

public static class WebApiKernel
{
    public static void Setup(IServiceCollection services)
    {
        services.AddSingleton<ProjectService>();
        services.AddSingleton<UserService>();
    }
}

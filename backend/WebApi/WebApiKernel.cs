using LanguageForge.WebApi.Services;

namespace LanguageForge.WebApi;

public static class WebApiKernel
{
    public static void Setup(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<ProjectService>();
        services.AddSingleton<UserService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ILfWebContext, LfWebContext>();
    }
}

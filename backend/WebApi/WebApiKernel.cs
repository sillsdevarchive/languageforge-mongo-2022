using LanguageForge.Api;
using LanguageForge.WebApi.Services;

namespace LanguageForge.WebApi;

public static class WebApiKernel
{
    public static void Setup(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddSingleton<ProjectService>();
        services.AddSingleton<UserService>();
        services.AddScoped<EntryService>();
        services.AddScoped<OptionsService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ILfWebContext, LfWebContext>();
        services.AddScoped<ILfProjectContext, LfProjectContext>();
    }
}

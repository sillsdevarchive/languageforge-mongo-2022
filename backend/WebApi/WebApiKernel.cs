using LanguageForge.WebApi.Services;

namespace LanguageForge.WebApi;

public static class WebApiKernel
{
    public static void Setup(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<ProjectService>();
        services.AddSingleton<UserService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ILfWebContext, LfWebContext>();
        services.AddScoped<ILfProjectContext, LfProjectContext>();
    }
}

using LanguageForge.WebApi.Auth;
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
        services.AddScoped((serviceProvider) =>
        {
            var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            if (httpContext?.User == null)
            {
                throw new InvalidOperationException($"{nameof(ILfWebContext)} should only be accessed in contexts where the user is authenticated.");
            }
            return JwtService.BuildUserContext(httpContext.User);
        });
    }
}

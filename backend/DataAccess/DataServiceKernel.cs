using LanguageForge.Api.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageForge.Api;

public static class DataServiceKernel
{
    public static void Setup(IServiceCollection services)
    {
        BsonConfiguration.Setup();
        services.AddSingleton<SystemDbContext>();
    }
}

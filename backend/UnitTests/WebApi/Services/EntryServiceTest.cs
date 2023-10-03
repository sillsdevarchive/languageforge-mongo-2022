using LanguageForge.UnitTests.Fixtures;
using LanguageForge.WebApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LanguageForge.UnitTests.WebApi.Services;

public class EntryServiceTest : IClassFixture<IntegrationTestFixture>
{
    private readonly EntryService _entryService;

    public EntryServiceTest(IntegrationTestFixture iocFixture)
    {
        _entryService = iocFixture.Services.GetRequiredService<EntryService>();
    }
}

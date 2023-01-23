using LanguageForge.Api.Entities;

namespace LanguageForge.WebApi.Auth;

public record LfUser(string Email, LfId<User> Id, UserRole Role, IReadOnlyList<UserProjectRole> Projects);

public record UserProjectRole(string ProjectCode, ProjectRole Role);

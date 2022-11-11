namespace LanguageForge.WebApi.Auth;

//todo setup auth to respect this attribute
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminRequiredAttribute : Attribute
{
}

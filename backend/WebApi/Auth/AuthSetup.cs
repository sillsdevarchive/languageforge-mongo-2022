using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

namespace LanguageForge.WebApi.Auth;

public static class AuthSetup
{
    public static void SetupLfAuth(this WebApplicationBuilder builder)
    {
        SetupLfAuth(builder.Services, builder.Configuration, builder.Environment);
    }

    public static void SetupLfAuth(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
        }
        services.AddSingleton<GoogleTokenValidator>();
        services.AddSingleton<JwtService>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(nameof(ProjectAuthorizationRequirement), policy => policy.Requirements.Add(new ProjectAuthorizationRequirement()));

            //fallback policy is used when there's no auth attribute.
            //default policy is when there's no parameters specified on the auth attribute
            //this will make sure that all endpoints require auth unless they have the AllowAnonymous attribute
            options.FallbackPolicy = AuthorizationPolicy.Combine(options.DefaultPolicy, options.GetPolicy(nameof(ProjectAuthorizationRequirement)));
        });
        services.AddScoped<IAuthorizationHandler, ProjectAuthorizationHandler>();
        services.AddOptions<JwtOptions>()
            .BindConfiguration("Authentication:Jwt")
            .Validate(options => options.GoogleClientId != "==== replace ====",
                "Jwt:GoogleClientId should have been changed from it's default value")
            .Validate(options => options.Secret != "==== replace ====",
                "Jwt:Secret should have been changed from it's default value")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = configuration.GetSection("Authentication:Jwt").Get<JwtOptions>();
                ArgumentNullException.ThrowIfNull(jwtOptions);
                if (jwtOptions.Secret == "==== replace ====")
                {
                    throw new ArgumentException("default jwt secret value used, please specify non default value");
                }
                options.Audience = jwtOptions.Audience;
                options.ClaimsIssuer = jwtOptions.Issuer;
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = JwtService.TokenValidationParameters(jwtOptions);
                options.MapInboundClaims = false;
            });


        services.ConfigureSwaggerGen(options =>
        {
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter the token from login, prefixed like this `Bearer {token}`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
    }
}

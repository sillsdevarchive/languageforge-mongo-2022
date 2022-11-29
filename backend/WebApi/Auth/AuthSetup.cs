using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

namespace LanguageForge.WebApi.Auth;

public static class AuthSetup
{
    public static void SetupLfAuth(this WebApplicationBuilder builder)
    {
        if (builder.Environment.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
        }
        builder.Services.AddSingleton<GoogleTokenValidator>();
        builder.Services.AddSingleton<JwtService>();
        builder.Services.AddAuthorization(options =>
        {
            //fallback policy is used when there's no auth attribute.
            //default policy is when there's no parameters specified on the auth attribute
            //this will make sure that all endpoints require auth unless they have the AllowAnonymous attribute
            options.FallbackPolicy = options.DefaultPolicy;
        });
        builder.Services.AddOptions<JwtOptions>()
            .BindConfiguration("Authentication:Jwt")
            .Validate(options => options.GoogleClientId != "==== replace ====",
                "Jwt:GoogleClientId should have been changed from it's default value")
            .Validate(options => options.Secret != "==== replace ====",
                "Jwt:Secret should have been changed from it's default value")
            .ValidateDataAnnotations()
            .ValidateOnStart();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtOptions = builder.Configuration.GetSection("Authentication:Jwt").Get<JwtOptions>();
                ArgumentNullException.ThrowIfNull(jwtOptions);
                if (jwtOptions.Secret == "==== replace ====")
                    throw new ArgumentException("default jwt secret value used, please specify non default value");
                options.Audience = jwtOptions.Audience;
                options.ClaimsIssuer = jwtOptions.Issuer;
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = JwtService.TokenValidationParameters(jwtOptions);
                options.MapInboundClaims = false;
            });


        builder.Services.ConfigureSwaggerGen(options =>
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

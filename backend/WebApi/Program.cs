using System.Text.Json.Serialization;
using LanguageForge.Api;
using LanguageForge.Api.Configuration;
using LanguageForge.Api.Entities;
using LanguageForge.WebApi;
using LanguageForge.WebApi.Auth;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    var enumConverter = new JsonStringEnumConverter();
    opts.JsonSerializerOptions.Converters.Add(enumConverter);
    opts.JsonSerializerOptions.Converters.Add(LfIdSerializerProvider.Instance);
    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType(typeof(LfId<>), () => new OpenApiSchema { Type = "string" });
});

DataServiceKernel.Setup(builder.Services);
WebApiKernel.Setup(builder.Services);
builder.SetupLfAuth();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI((settings) =>
{
    if (app.Environment.IsDevelopment())
    {
        settings.EnableTryItOutByDefault();
    }
});


app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

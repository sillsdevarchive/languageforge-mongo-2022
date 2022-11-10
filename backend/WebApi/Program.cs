using LanguageForge.Api;
using LanguageForge.Api.Configuration;
using LanguageForge.WebApi;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DataServiceKernel.Setup(builder.Services);
WebApiKernel.Setup(builder.Services);

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

app.UseAuthorization();

app.MapControllers();

app.Run();

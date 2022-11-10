using LanguageForge.Api;
using LanguageForge.Api.Configuration;
using LanguageForge.Api.Services;
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
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

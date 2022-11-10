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

var mongoSettings = MongoClientSettings.FromConnectionString(builder.Configuration.GetValue<string>("Mongo:ConnectionString"));
mongoSettings.LinqProvider = LinqProvider.V3;
mongoSettings.ConnectTimeout = TimeSpan.FromSeconds(1);
mongoSettings.SocketTimeout = TimeSpan.FromSeconds(1);
mongoSettings.HeartbeatTimeout = TimeSpan.FromSeconds(2);
builder.Services.AddSingleton(mongoSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

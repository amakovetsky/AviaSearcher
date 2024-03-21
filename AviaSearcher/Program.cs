using WebAPI;
using Serilog;
using WebAPI.Clients;
using WebAPI.Redis;
using AviaSearcher.AviaSearchService;
using AviaSearcher;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddMainConfigureServices(builder.Configuration);
builder.Services.AddRedisServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Host.UseSerilog();


var app = builder
    .Build()
    .MainConfigure();

app.Run();


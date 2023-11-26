using Shared.Domain.Entities;
using Shared.Extensions.Configurations;
using Shared.Extensions.DependencyInjection;
using Updater;
using Updater.Configurations;
using Updater.Repositories.Technology;
using Updater.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IKafkaService, KafkaService>();

builder.Services
    .AddOptions<MongoConfiguration>()
    .BindConfiguration("Mongo");

builder.Services
    .AddMongo(builder.Configuration.GetSection("Mongo").Get<MongoConfiguration>()!)
    .AddCollection<Technology, ITechnologyRepository, TechnologyRepository>("technologies");

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
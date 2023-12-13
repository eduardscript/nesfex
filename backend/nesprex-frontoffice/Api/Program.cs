using Api.Mutations;
using Api.Mutations.Types;
using Api.Queries;
using Infra.MongoDb;
using Shared.Extensions.DependantServices.DependencyInjection;
using Shared.Extensions.Mongo.DependencyInjection;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddMutationType<Mutation>()
    .AddType<TechnologyMutationType>()
    .AddQueryType<Query>();

builder.Services
    .AddBackofficeClient()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri("http://localhost:5199/graphql"));

builder.Services
    .AddSingleton<ITechnologyClient, TechnologyClient>();

builder.Services
    .AddInfraMongoDb(builder.Configuration);

var app = builder.Build();

app.MapGraphQL();

app.Run();

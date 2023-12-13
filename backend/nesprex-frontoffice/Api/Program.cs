using Api.Configurations;
using Api.Mutations;
using Api.Mutations.Types;
using Api.Queries;
using Shared.Extensions.DependantServices.DependencyInjection;

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
    .AddDependantServices(builder.Configuration);

builder.Services
    .AddOptions<DependantServicesConfiguration>()
    .Bind(builder.Configuration.GetSection("DependantServices"));

var app = builder.Build();

app.MapGraphQL();

app.Run();

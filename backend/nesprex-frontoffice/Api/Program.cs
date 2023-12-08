using Api.Mutations;
using Api.Queries;
using Api.Repositories;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddMutationType<Mutation>()
    .AddType<Mutation.MutationType>()
    .AddQueryType<Query>();

builder.Services
    .AddSingleton<IUserTechnologiesRepository, TempUserTechnologiesRepository>();

var app = builder.Build();

app.MapGraphQL();

app.Run();

using Api.Queries;
using Infra.MongoDb;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<Query.TechnologyType>();

builder.Services.AddInfraMongoDb(builder.Configuration);

var app = builder.Build();

app.MapGraphQL();

app.Run();
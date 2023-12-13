using Api.Queries;
using Infra.MongoDb;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddType<Query.TechnologyType>();

builder.Services
    .AddInfraMongoDb(builder.Configuration);

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseCors(policyBuilder => policyBuilder
        .WithOrigins("http://localhost:5018")
        .WithMethods("GET"));
}

app.MapGet("/ping", () => "pong");

app.MapGraphQL();

app.Run();
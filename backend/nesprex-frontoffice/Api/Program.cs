using System.Text;
using Api.Clients;
using Api.Mutations;
using Api.Mutations.Types;
using Api.Queries;
using Api.Services;
using Infra.MongoDb;
using Microsoft.IdentityModel.Tokens;
using Shared.Extensions.DependantServices.DependencyInjection;
using Shared.Extensions.Mongo.DependencyInjection;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddMutationType<Mutation>()
    .AddType<AuthMutation>()
    .AddType<TechnologyMutationType>()
    .AddQueryType<Query>();

builder.Services
    .AddSingleton<IAuthService, AuthService>();

var signingKey = new SymmetricSecurityKey(
    "ff842af0677a2c1e84cf8954d4d67dc73791e80c4f10e4460fc4dd71de70cbcc"u8.ToArray());

builder.Services
    .AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.Authority = "Api";
        options.Audience = "Api";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new()
        {
            ValidIssuer = "Api",
            ValidAudience = "Api",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
        };
    });

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


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OrganizerApi.Auth.AuthService;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using OrganizerApi.Auth.UserService;
using OrganizerApi.Auth.Repository;
using OrganizerApi.Cookbook.CookRepository;
using OrganizerApi.Cookbook.CookServices;
using OrganizerApi.Cookbook.Config;
using OrganizerApi.Auth.Config;
using Microsoft.Azure.Cosmos;
using OrganizerApi.Diary.Repository;
using OrganizerApi.Diary.DiaryServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Build configuration
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Configure Cosmos DB settings for Cookbook
var cosmosDbCookbookConfig = new CookbookConfigDTO();
configuration.GetSection("CosmosDBCookbook").Bind(cosmosDbCookbookConfig);

var cosmosCookbookClient = new CosmosClient(cosmosDbCookbookConfig.EndpointUri, cosmosDbCookbookConfig.PrimaryKey);

builder.Services.AddSingleton(cosmosCookbookClient);

builder.Services.AddScoped<ICookBookRepository>(sp =>
    new CookBookRepository(
        cosmosCookbookClient,
        cosmosDbCookbookConfig.DatabaseId,
        cosmosDbCookbookConfig.ContainerId
    ));

// Configure Cosmos DB settings for Auth
var cosmosDbAuthConfig = new ConfigDTO();
configuration.GetSection("CosmosDBAuth").Bind(cosmosDbAuthConfig);

var cosmosAuthClient = new CosmosClient(cosmosDbAuthConfig.EndpointUri, cosmosDbAuthConfig.PrimaryKey);

builder.Services.AddSingleton(cosmosAuthClient);

builder.Services.AddScoped<IUserRepository>(sp =>
    new UserRepository(
        cosmosAuthClient,
        cosmosDbAuthConfig.DatabaseId,
        cosmosDbAuthConfig.ContainerId
    ));

var cosmosDbDiaryConfig = new ConfigDTO();
configuration.GetSection("CosmosDBDiary").Bind(cosmosDbDiaryConfig);

var cosmosDiaryClient = new CosmosClient(cosmosDbDiaryConfig.EndpointUri, cosmosDbDiaryConfig.PrimaryKey);

builder.Services.AddSingleton(cosmosDiaryClient);

builder.Services.AddScoped<IDiaryRepository>(sp =>
    new DiaryRepository(
        cosmosDiaryClient,
        cosmosDbDiaryConfig.DatabaseId,
        cosmosDbDiaryConfig.ContainerId
    ));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                "2e7b57ad2498e34f0b6405bb29c9959fcf63f2f89dc13e27a4dd524d9c16d9e2"))
    };
});

// cors settings
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:3000", // ändra address
                                                  "http://localhost:5007",
                                                  "https://localhost:7066",
                                                  "https://ashy-cliff-060617603.3.azurestaticapps.net")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<ICookBookService, CookbookService>();
builder.Services.AddScoped<IMealService, MealService>();
builder.Services.AddScoped<IShoppinglistService, ShoppinglistService>();

builder.Services.AddScoped<IDiaryService, DiaryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins); // prova flytta runt

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

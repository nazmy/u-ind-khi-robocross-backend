using System.Text.Json.Serialization;
using Azure.Storage;
using Azure.Storage.Blobs;
using Domain.Helper;
using khi_robocross_api.Health;
using khi_robocross_api.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System.Security.Claims;
using System.Text;
using Casbin.AspNetCore.Authorization;
using Casbin.AspNetCore.Authorization.Transformers;
using Casbin.Persist.Adapter.EFCore;
using Casbin;
using domain.Identity.Manager;
using domain.Repositories;
using domain.Repositories.Config;
using domain.Repositories.Manager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Logging.ClearProviders();

//Healthcheck
builder.Services.AddHealthChecks().AddCheck<MongoHealthCheck>("DBConnectionCheck");

var connectionString = builder.Configuration.GetValue<string>("RobocrossDatabaseSettings:ConnectionString");
var databaseName = builder.Configuration.GetValue<string>("RobocrossDatabaseSettings:DatabaseName");

// Add Robocross MongoDB 
builder.Services.Configure<RobocrossDatabaseSettings>(
    builder.Configuration.GetSection(nameof(RobocrossDatabaseSettings)));
builder.Services.AddSingleton<IRobocrossDatabaseSettings>(sp =>
sp.GetRequiredService<IOptions<RobocrossDatabaseSettings>>().Value); 
builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(connectionString));


builder.Services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredUniqueChars = 1;
            
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 10;

            // ApplicationUser settings
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@.-_";
        }
    )
    .AddMongoDbStores<AppUser, AppRole, ObjectId>
    (
        connectionString,databaseName
    )
    .AddDefaultTokenProviders();

JwtTokenConfig jwtTokenConfig = builder.Configuration.GetRequiredSection("JwtTokenSettings").Get<JwtTokenConfig>();
builder.Services.AddSingleton(jwtTokenConfig);
builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtTokenConfig.Issuer,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidAudience = jwtTokenConfig.Audience,
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

//Add MongoDB Migration    
// builder.Services.AddMigration(new MongoMigrationSettings
//  {
//      ConnectionString = builder.Configuration.GetValue<string>("RobocrossDatabaseSettings:ConnectionString"),
//      Database = builder.Configuration.GetValue<string>("RobocrossDatabaseSettings:DatabaseName"),
//      VersionFieldName = "1.0.0"
//  });


//Add Robocross Azure Blob Storage
builder.Services.Configure<RobocrossBlobStorageSettings>(
    builder.Configuration.GetSection(nameof(RobocrossBlobStorageSettings)));
builder.Services.AddSingleton<IRobocrossBlobStorageSettings>(sp =>
    sp.GetRequiredService<IOptions<RobocrossBlobStorageSettings>>().Value);
builder.Services.AddAzureClients(clientBuiler =>
{
    var accountName = builder.Configuration.GetValue<string>("RobocrossBlobStorageSettings:AcountName");
    var accountKey = builder.Configuration.GetValue<string>("RobocrossBlobStorageSettings:Key");
    StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);
    var blobUri = String.Concat("https://").Concat(accountName).Concat("blob.core.windows.net").ToString();
    clientBuiler.AddBlobServiceClient(blobUri);
});

builder.Services.AddSingleton<BlobServiceClient>(s => 
    new BlobServiceClient(builder.Configuration.GetValue<string>("RobocrossBlobStorageSettings:ConnectionString")));

//Add NewtonJson Serializer
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);


//Add Service and Repository 
builder.Services.AddScoped<IClientService,ClientService>();
builder.Services.AddScoped<IBuildingService,BuildingService>();
builder.Services.AddScoped<ILineService,LineService>();
builder.Services.AddScoped<IAssetManagerService, AssetManagerService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICompoundRepository, CompoundRepository>();
builder.Services.AddScoped<ICompoundService, CompoundService>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<ILineRepository, LineRepository>();
builder.Services.AddScoped<IMessageRepository,  MessageRepository>();

builder.Services.AddSingleton<IJwtAuthManager, JwtAuthManager>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//Add Casbin Authorizer, CasbinDB  
builder.Services.AddCasbinAuthorization(options =>
{
    options.PreferSubClaimType = ClaimTypes.Name;
    options.DefaultModelPath = "./Enforcer/rbac_with_domain_pattern_model.conf";
    options.DefaultPolicyPath = "./Enforcer/rbac_with_domain_pattern_policy.csv";

    options.DefaultEnforcerFactory = (p, m) =>
        new Enforcer(m, new EFCoreAdapter<string>(p.GetRequiredService<CasbinDbContext<string>>()));
    options.DefaultRequestTransformerType = typeof(BasicRequestTransformer);
});


//// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KHI Robocross API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // must be lower case
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });
});

builder.Services.AddHttpContextAccessor();

// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowAll",
//         builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
// });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCasbinAuthorization();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healtz");

app.Run();


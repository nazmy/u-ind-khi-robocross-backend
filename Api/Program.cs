using System.Text.Json.Serialization;
using Azure.Storage;
using Azure.Storage.Blobs;
using Domain.Helper;
using khi_robocross_api.Health;
using khi_robocross_api.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();
builder.Logging.ClearProviders();
builder.Services.AddHealthChecks().AddCheck<MongoHealthCheck>("DBConnectionCheck");

// Add services to the container.
builder.Services.Configure<RobocrossDatabaseSettings>(
    builder.Configuration.GetSection(nameof(RobocrossDatabaseSettings)));

builder.Services.AddSingleton<IRobocrossDatabaseSettings>(sp =>
sp.GetRequiredService<IOptions<RobocrossDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("RobocrossDatabaseSettings:ConnectionString")));

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

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddScoped<IClientService,ClientService>();
builder.Services.AddScoped<IBuildingService,BuildingService>();
builder.Services.AddScoped<ILineService,LineService>();
builder.Services.AddScoped<IAssetManagerService, AssetManagerService>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ICompoundRepository, CompoundRepository>();
builder.Services.AddScoped<ICompoundService, CompoundService>();
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<ILineRepository, LineRepository>();


builder.Services.AddControllers().AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healtz");

app.Run();


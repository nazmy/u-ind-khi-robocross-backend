
using Domain.Helper;
using khi_robocross_api.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<RobocrossDatabaseSettings>(
    builder.Configuration.GetSection(nameof(RobocrossDatabaseSettings)));

builder.Services.AddSingleton<IRobocrossDatabaseSettings>(sp =>
sp.GetRequiredService<IOptions<RobocrossDatabaseSettings>>().Value);

builder.Services.AddSingleton<IMongoClient>(s =>
new MongoClient(builder.Configuration.GetValue<string>("RobocrossDatabaseSettings:ConnectionString")));

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientService,ClientService>();

builder.Services.AddScoped<ICompoundRepository, CompoundRepository>();
builder.Services.AddScoped<ICompoundService, CompoundService>();

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

app.Run();


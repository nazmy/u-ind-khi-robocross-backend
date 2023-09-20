using Domain.Helper;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace khi_robocross_api.Health;

public class MongoHealthCheck : IHealthCheck
{
    private IMongoDatabase _db { get; set; }
    
    
    public MongoHealthCheck(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
    {
        _db = mongoClient.GetDatabase(settings.DatabaseName);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var healthCheckResultHealthy = await CheckMongoDBConnectionAsync();
        if (healthCheckResultHealthy)
        {
            return HealthCheckResult.Healthy("DB Connection is healthy");
        }
        return HealthCheckResult.Unhealthy("DB Connection failure");
    }
    
    private async Task<bool> CheckMongoDBConnectionAsync()
    {
        try
        {
            await _db.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
        }
        catch (Exception)
        {
            return false;
        }
 
        return true;
    }
}
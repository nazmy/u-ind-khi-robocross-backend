using Domain.Entities;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace domain.Repositories;

public class BuildingRepository : IBuildingRepository
{
	private readonly IMongoCollection<Building> _building;

	public BuildingRepository(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
	{
		var database = mongoClient.GetDatabase(settings.DatabaseName);
		_building = database.GetCollection<Building>(settings.BuildingsCollectionName);

		var indexKeysDefinition = Builders<Building>
			.IndexKeys.Descending(x => x.LastUpdatedAt);
		_building.Indexes.CreateOneAsync(new CreateIndexModel<Building>(indexKeysDefinition));
	}
	

	public async Task CreateAsync(Building building) =>
		await _building.InsertOneAsync(building);

	public async Task<IEnumerable<Building>> GetAsync(DateTimeOffset? lastUpdatedAt)
	{
		if (lastUpdatedAt != null)
		{
			var filter = Builders<Building>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			return await _building.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
		}
		else
		{
			return await _building.Find(_ => true).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
		}
	}
		

	public async Task<Building?> GetAsync(string id)
	{
		return await _building.Find(x => x.Id == id).FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<Building>> SearchAsync(string search)
	{
		var filter = Builders<Building>.Filter.Empty;
		if (!string.IsNullOrEmpty((search)))
		{
			filter = Builders<Building>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
		}
		return await _building.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
	}

	public async Task<IEnumerable<Building>> GetAsyncByCompoundId(string compoundId, DateTimeOffset? lastUpdatedAt)
	{
		if (lastUpdatedAt != null)
		{
			var filter = Builders<Building>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			filter &= Builders<Building>.Filter.Eq(x => x.CompoundId, compoundId);
			return await _building.Find(filter) .SortByDescending(b => b.LastUpdatedAt).ToListAsync();
		}
		else
		{
			return await _building.Find(x => x.CompoundId == compoundId).SortByDescending(b => b.LastUpdatedAt).ToListAsync();	
		}
	}
		

	public async Task RemoveAsync(string id, string? usernameActor)
	{
		await _building.UpdateOneAsync<Building>(b => b.Id == id,
			Builders<Building>.Update
				.Set(b => b.IsDeleted, true)
				.Set(b => b.LastUpdatedAt,DateTimeOffset.UtcNow)
			);
	}
		

	public async Task UpdateAsync(string id, Building updatedBuilding) =>
		await _building.ReplaceOneAsync(x => x.Id == id, updatedBuilding);
}


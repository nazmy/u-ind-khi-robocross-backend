using Domain.Entities;
using Domain.Helper;
using domain.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using NLog.Filters;

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

	private FilterDefinition<Building> GetFilterByLoggedInUser(LoggedInUser loggedInUser)
	{
		FilterDefinition<Building> filter = FilterDefinition<Building>.Empty;
		if (loggedInUser.Role == "Customer")
		{
			filter = Builders<Building>.Filter.Eq(x => x.ClientId, loggedInUser.ClientId);	
		}

		return filter;
	} 

	public async Task<IEnumerable<Building>> GetAsync(LoggedInUser loggedInUser, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
	{
		var filter = GetFilterByLoggedInUser(loggedInUser);
		
		if (lastUpdatedAt != null)
		{
			filter &= Builders<Building>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			if (isDeleted == false)
			{
				filter &= Builders<Building>.Filter.Eq(x => x.IsDeleted , false);
			}
			return await _building.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
		}
		else
		{
			if (isDeleted == null || isDeleted == true)
			{
				return await _building.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
			}
			else
			{
				filter &= Builders<Building>.Filter.Eq(x => x.IsDeleted , false);
				return await _building.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
			}
		}
	}
	
	public async Task<Building?> GetAsync(LoggedInUser loggedInUser, string id)
	{
		var filter = GetFilterByLoggedInUser(loggedInUser);
		filter &= Builders<Building>.Filter.Eq(x => x.Id, id);
		return await _building.Find(filter).FirstOrDefaultAsync();
	}

	public async Task<IEnumerable<Building>> SearchAsync(LoggedInUser loggedInUser, string search)
	{
		var filter = GetFilterByLoggedInUser(loggedInUser);
		if (!string.IsNullOrEmpty((search)))
		{
			filter &= Builders<Building>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
		}
		return await _building.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
	}

	public async Task<IEnumerable<Building>> GetAsyncByCompoundId(LoggedInUser loggedInUser, string compoundId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
	{
		var filter = GetFilterByLoggedInUser(loggedInUser);
		if (lastUpdatedAt != null)
		{
			filter &= Builders<Building>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			filter &= Builders<Building>.Filter.Eq(x => x.CompoundId, compoundId);
			if (isDeleted == false)
			{
				filter &= Builders<Building>.Filter.Eq(x => x.IsDeleted , false);
			}
			return await _building.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
		}
		else
		{
			filter &= Builders<Building>.Filter.Eq(x => x.CompoundId, compoundId);
			if (isDeleted == null || isDeleted == true)
			{
				filter &= Builders<Building>.Filter.Eq(x => x.IsDeleted , false);
			}
			
			return await _building.Find(filter).SortByDescending(b => b.LastUpdatedAt)
				.ToListAsync();
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


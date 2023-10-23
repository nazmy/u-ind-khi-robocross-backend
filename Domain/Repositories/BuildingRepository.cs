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
	}

	public async Task CreateAsync(Building building) =>
		await _building.InsertOneAsync(building);

	public async Task<IEnumerable<Building>> GetAsync() =>
		await _building.Find(_ => true).ToListAsync();

	public async Task<Building?> GetAsync(string id) =>
		await _building.Find(x => x.Id == id).FirstOrDefaultAsync();

	public async Task<IEnumerable<Building>> SearchAsync(string search)
	{
		var filter = Builders<Building>.Filter.Empty;
		if (!string.IsNullOrEmpty((search)))
		{
			filter = Builders<Building>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
		}
		return await _building.Find(filter).ToListAsync();
	}

	public async Task<IEnumerable<Building>> GetAsyncByCompoundId(string compoundId) =>
		await _building.Find(x => x.CompoundId == compoundId).ToListAsync();

	public async Task RemoveAsync(string id) =>
		await _building.DeleteOneAsync(x => x.Id == id);

	public async Task UpdateAsync(string id, Building updatedBuilding) =>
		await _building.ReplaceOneAsync(x => x.Id == id, updatedBuilding);
}


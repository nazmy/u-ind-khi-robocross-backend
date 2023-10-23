using Domain.Entities;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace domain.Repositories;

public class TimelineRepository : ITimelineRepository
{
	private readonly IMongoCollection<Timeline> _timeline;

	public TimelineRepository(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
	{
		var database = mongoClient.GetDatabase(settings.DatabaseName);
		_timeline = database.GetCollection<Timeline>(settings.TimelinesCollectionName);
	}

	public async Task CreateAsync(Timeline timeline) =>
		await _timeline.InsertOneAsync(timeline);

	public async Task<IEnumerable<Timeline>> GetAsync() =>
		await _timeline.Find(_ => true).ToListAsync();

	public async Task<Timeline?> GetAsync(string id) =>
		await _timeline.Find(x => x.Id == id).FirstOrDefaultAsync();

	public async Task<IEnumerable<Timeline>> SearchAsync(string search)
	{
		var filter = Builders<Timeline>.Filter.Empty;
		if (!string.IsNullOrEmpty((search)))
		{
			filter = Builders<Timeline>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
		}
		return await _timeline.Find(filter).ToListAsync();
	}

	public async Task<IEnumerable<Timeline>> GetAsyncByUnitId(string unitId) =>
		await _timeline.Find(x => x.UnitId == unitId).ToListAsync();

	public async Task RemoveAsync(string id) =>
		await _timeline.DeleteOneAsync(x => x.Id == id);

	public async Task UpdateAsync(string id, Timeline updatedTimeline) =>
		await _timeline.ReplaceOneAsync(x => x.Id == id, updatedTimeline);
}


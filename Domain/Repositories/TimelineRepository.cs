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
		
		var indexKeysDefinition = Builders<Timeline>
			.IndexKeys.Descending(x => x.LastUpdatedAt);
		_timeline.Indexes.CreateOneAsync(new CreateIndexModel<Timeline>(indexKeysDefinition));
	}

	public async Task CreateAsync(Timeline timeline) =>
		await _timeline.InsertOneAsync(timeline);

	public async Task<IEnumerable<Timeline>> GetAsync(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
	{
		if (lastUpdatedAt != null)
		{
			var filter = Builders<Timeline>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			if (isDeleted == false)
			{
				filter &= Builders<Timeline>.Filter.Eq(x => x.IsDeleted , false);
			}
			return await _timeline.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
		}
		else
		{
			if (isDeleted == null || isDeleted == true)
			{
				return await _timeline.Find(_ => true).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
			else
			{
				return await _timeline.Find(x => x.IsDeleted == false).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
		}
	}

	public async Task<Timeline?> GetAsync(string id) =>
		await _timeline.Find(x => x.Id == id).FirstOrDefaultAsync();

	public async Task<IEnumerable<Timeline>> SearchAsync(string search)
	{
		var filter = Builders<Timeline>.Filter.Empty;
		if (!string.IsNullOrEmpty((search)))
		{
			filter = Builders<Timeline>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
		}
		return await _timeline.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
	}

	public async Task<IEnumerable<Timeline>> GetAsyncByUnitId(string unitId,DateTimeOffset? lastUpdatedAt, bool? isDeleted)
	{
		if (lastUpdatedAt != null)
		{
			var filter = Builders<Timeline>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			filter &= Builders<Timeline>.Filter.Eq(x => x.UnitId, unitId);
			if (isDeleted == false)
			{
				filter &= Builders<Timeline>.Filter.Eq(x => x.IsDeleted , false); 
			}
			return await _timeline.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
		}
		else
		{
			var filter = Builders<Timeline>.Filter.Eq(x => x.UnitId, unitId);
			if (isDeleted == false)
			{
				filter &= Builders<Timeline>.Filter.Eq(x => x.IsDeleted , false); 
			}
			return await _timeline.Find(filter)
            			.SortByDescending(x => x.LastUpdatedAt).ToListAsync();
		}
	}

	public async Task RemoveAsync(string id, string? usernameActor) =>
		await _timeline.UpdateOneAsync(timeline => timeline.Id == id,
			Builders<Timeline>.Update
				.Set(timeline => timeline.IsDeleted, true)
				.Set(timeline => timeline.LastUpdatedAt,DateTimeOffset.UtcNow));

	public async Task UpdateAsync(string id, Timeline updatedTimeline) =>
		await _timeline.ReplaceOneAsync(x => x.Id == id, updatedTimeline);
}


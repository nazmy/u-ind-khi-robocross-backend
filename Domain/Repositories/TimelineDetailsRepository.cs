using Domain.Entities;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace domain.Repositories;

public class TimelineDetailsRepository : ITimelineDetailsRepository
{
	private readonly IMongoCollection<TimelineDetails> _timelineDetails;

	public TimelineDetailsRepository(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
	{
		var database = mongoClient.GetDatabase(settings.DatabaseName);
		_timelineDetails = database.GetCollection<TimelineDetails>(settings.TimelineDetailsCollectionName);
		
		var indexKeysDefinition = Builders<TimelineDetails>
			.IndexKeys.Descending(x => x.LastUpdatedAt);
		_timelineDetails.Indexes.CreateOneAsync(new CreateIndexModel<TimelineDetails>(indexKeysDefinition));
	}

	public async Task CreateAsync(TimelineDetails timelineDetails) =>
		await _timelineDetails.InsertOneAsync(timelineDetails);
	
	public async Task<BulkWriteResult<TimelineDetails>> CreateManyAsync(List<WriteModel<TimelineDetails>> timelineDetails)
	{
		var writeResult = await _timelineDetails.BulkWriteAsync(timelineDetails);
		return writeResult;
	}

	public async Task<IEnumerable<TimelineDetails>> GetAsync(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
	{
		if (lastUpdatedAt != null)
		{
			var filter = Builders<TimelineDetails>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
			if (isDeleted == false)
			{
				filter &= Builders<TimelineDetails>.Filter.Eq(x => x.IsDeleted , false);
			}
			return await _timelineDetails.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
		}
		else
		{
			if (isDeleted == null || isDeleted == true)
			{
				return await _timelineDetails.Find(_ => true).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
			else
			{
				return await _timelineDetails.Find(x => x.IsDeleted == false).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
		}
	}

	public async Task<List<TimelineDetails>> GetManyAsync(List<string> ids)
	{
		var filter = Builders<TimelineDetails>.Filter.Empty;
		foreach (var id in ids)
		{
			filter |= Builders<TimelineDetails>.Filter.Eq(x => x.Id, id);
		}

		return _timelineDetails.Find(filter).ToList();
	}

	public async Task<TimelineDetails?> GetAsync(string id) =>
		await _timelineDetails.Find(x => x.Id == id).FirstOrDefaultAsync();

	public async Task<IEnumerable<TimelineDetails>> SearchAsync(string search)
	{
		var filter = Builders<TimelineDetails>.Filter.Empty;
		if (!string.IsNullOrEmpty((search)))
		{
			filter = Builders<TimelineDetails>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
		}
		return await _timelineDetails.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
	}
	
	public async Task RemoveAsync(string id, string? usernameActor) =>
		await _timelineDetails.UpdateOneAsync(timelineDetails => timelineDetails.Id == id,
			Builders<TimelineDetails>.Update
				.Set(timeline => timeline.IsDeleted, true)
				.Set(timeline => timeline.LastUpdatedAt,DateTimeOffset.UtcNow));

	public async Task UpdateAsync(string id, TimelineDetails updatedTimelineDetails) =>
		await _timelineDetails.ReplaceOneAsync(x => x.Id == id, updatedTimelineDetails);
}


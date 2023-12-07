using Domain.Entities;
using MongoDB.Driver;

namespace domain.Repositories
{
	public interface ITimelineDetailsRepository
	{
		Task<IEnumerable<TimelineDetails>> GetAsync(DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		Task<TimelineDetails> GetAsync(string id);
		
		Task<List<TimelineDetails>> GetManyAsync(List<string> ids);
		Task<IEnumerable<TimelineDetails>> SearchAsync(string search);
		Task CreateAsync(TimelineDetails timeline);
		Task<BulkWriteResult<TimelineDetails>> CreateManyAsync(List<WriteModel<TimelineDetails>> timelineDetails);
		Task UpdateAsync(string id, TimelineDetails updatedTimelineDetails);
		Task RemoveAsync(string id, string? usernameActor);
    }
}


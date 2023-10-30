using Domain.Entities;

namespace domain.Repositories
{
	public interface ITimelineRepository
	{
		Task<IEnumerable<Timeline>> GetAsync(DateTimeOffset? lastUpdatedAt);
		Task<Timeline> GetAsync(string id);
		Task<IEnumerable<Timeline>> SearchAsync(string search);
		Task CreateAsync(Timeline timeline);
		Task UpdateAsync(string id, Timeline updatedTimeline);
		Task RemoveAsync(string id, string? usernameActor);
        Task<IEnumerable<Timeline>> GetAsyncByUnitId(string unitId,DateTimeOffset? lastUpdatedAt);
    }
}


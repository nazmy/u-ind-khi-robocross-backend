using Domain.Entities;

namespace domain.Repositories
{
	public interface ILineRepository
	{
		Task<IEnumerable<Line>> GetAsync(DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		Task<Line> GetAsync(string id);
		Task<IEnumerable<Line>> SearchAsync(string search);
		Task CreateAsync(Line line);
		Task UpdateAsync(string id, Line updatedLine);
		Task RemoveAsync(string id, string? usernameActor);
        Task<IEnumerable<Line>> GetAsyncByBuildingId(string buildingId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
        Task<IEnumerable<Line>> GetAsyncByIntegratorId(string integratorId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
    }
}


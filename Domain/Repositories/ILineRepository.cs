using Domain.Entities;
using domain.Identity;

namespace domain.Repositories
{
	public interface ILineRepository
	{
		Task<IEnumerable<Line>> GetAsync(LoggedInUser loggedInUser, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		Task<Line> GetAsync(LoggedInUser loggedInUser, string id);
		Task<IEnumerable<Line>> SearchAsync(LoggedInUser loggedInUser, string search);
		Task CreateAsync(Line line);
		Task UpdateAsync(string id, Line updatedLine);
		Task RemoveAsync(string id, string? usernameActor);
        Task<IEnumerable<Line>> GetAsyncByBuildingId(LoggedInUser loggedInUser, string buildingId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
        Task<IEnumerable<Line>> GetAsyncByIntegratorId(LoggedInUser loggedInUser, string integratorId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
    }
}


using Domain.Entities;
using domain.Identity;

namespace domain.Repositories
{
	public interface IBuildingRepository
	{
		Task<IEnumerable<Building>> GetAsync(LoggedInUser loggedInUser, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		Task<Building> GetAsync(LoggedInUser loggedInUser, string id);
		Task<IEnumerable<Building>> SearchAsync(LoggedInUser loggedInUser, string search);
		Task CreateAsync(Building building);
		Task UpdateAsync(string id, Building updatedBuilding);
		Task RemoveAsync(string id,string? usernameActor);
        Task<IEnumerable<Building>> GetAsyncByCompoundId(LoggedInUser loggedInUser, string compoundId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
    }
}


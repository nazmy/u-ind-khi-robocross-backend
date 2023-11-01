using Domain.Entities;

namespace domain.Repositories
{
	public interface IBuildingRepository
	{
		Task<IEnumerable<Building>> GetAsync(DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		Task<Building> GetAsync(string id);
		Task<IEnumerable<Building>> SearchAsync(string search);
		Task CreateAsync(Building building);
		Task UpdateAsync(string id, Building updatedBuilding);
		Task RemoveAsync(string id,string? usernameActor);
        Task<IEnumerable<Building>> GetAsyncByCompoundId(string compoundId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
    }
}


using Domain.Entities;

namespace domain.Repositories
{
	public interface ICompoundRepository
	{
		Task<IEnumerable<Compound>> GetAsync(DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		Task<Compound> GetAsync(string id);
		Task<IEnumerable<Compound>> SearchAsync(string search);
		Task CreateAsync(Compound compound);
		Task UpdateAsync(string id, Compound updatedCompound);
		Task RemoveAsync(string id, string? usernameActor);
        Task<IEnumerable<Compound>> GetAsyncByClientId(string clientId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
    }
}


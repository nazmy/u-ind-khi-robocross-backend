using Domain.Entities;
using domain.Identity;

namespace domain.Repositories
{
	public interface ICompoundRepository
	{
		Task<IEnumerable<Compound>> GetAsync(LoggedInUser loggedInUser, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		Task<Compound> GetAsync(LoggedInUser loggedInUser, string id);
		Task<IEnumerable<Compound>> SearchAsync(LoggedInUser loggedInUser, string search);
		Task CreateAsync(Compound compound);
		Task UpdateAsync(string id, Compound updatedCompound);
		Task RemoveAsync(string id, string? usernameActor);
        Task<IEnumerable<Compound>> GetAsyncByClientId(LoggedInUser loggedInUser, string clientId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
    }
}


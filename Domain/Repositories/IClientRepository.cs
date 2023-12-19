using Domain.Entities;
using domain.Identity;

namespace domain.Repositories
{
	public interface IClientRepository
	{
		Task<IEnumerable<Client>> GetAsync(LoggedInUser loggedInUser, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		Task<Client> GetAsync(LoggedInUser loggedInUser, String id);
		Task<IEnumerable<Client>> SearchAsync(LoggedInUser loggedInUser, string search);
		Task CreateAsync(Client client);
		Task UpdateAsync(string id, Client updatedClient);
		Task RemoveAsync(string id, string? usernameActor);
	}
}


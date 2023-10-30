using Domain.Entities;

namespace domain.Repositories
{
	public interface IClientRepository
	{
		Task<IEnumerable<Client>> GetAsync(DateTimeOffset? lastUpdatedAt);
		Task<Client> GetAsync(String id);
		Task<IEnumerable<Client>> SearchAsync(string search);
		Task CreateAsync(Client client);
		Task UpdateAsync(string id, Client updatedClient);
		Task RemoveAsync(string id, string? usernameActor);
	}
}


using System;
using Domain.Entities;
using MongoDB.Driver;
using Domain.Dto;

namespace khi_robocross_api.Services
{
	public interface IClientRepository
	{
		Task<IEnumerable<Client>> GetAsync();
		Task<Client> GetAsync(String id);
		Task CreateAsync(Client client);
		Task UpdateAsync(string id, Client updatedClient);
		Task RemoveAsync(string id);
	}
}


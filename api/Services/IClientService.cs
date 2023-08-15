using System;
using Domain.Models;
using MongoDB.Driver;

namespace khi_robocross_api.Services
{
	public interface IClientService
	{
		Task<List<Client>> GetAsync();
		Task<Client> GetAsync(String id);
		Task CreateAsync(Client client);
	}
}


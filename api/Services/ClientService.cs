using System;
using Domain.Models;
using MongoDB.Driver;
namespace khi_robocross_api.Services
{
	public class ClientService : IClientService
	{
		private readonly IMongoCollection<Client> _clients;

		public ClientService(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);
			_clients = database.GetCollection<Client>(settings.ClientsCollectionName);
		}

		public async Task CreateAsync(Client client) =>
			await _clients.InsertOneAsync(client);

		public async Task<List<Client>> GetAsync() =>
			await _clients.Find(_ => true).ToListAsync();

		public async Task<Client?> GetAsync(string id) =>
			await _clients.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task RemoveAsync(string id) =>
			await _clients.DeleteOneAsync(x => x.Id == id);

		public async Task UpdateAsync(string id, Client updatedClient) =>
			await _clients.ReplaceOneAsync(x => x.Id == id, updatedClient);
    }
}

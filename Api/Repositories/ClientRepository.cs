using System;
using Domain.Entities;
using Domain.Helper;
using MongoDB.Driver;
using Domain.Dto;

namespace khi_robocross_api.Services
{
	public class ClientRepository : IClientRepository
	{
		private readonly IMongoCollection<Client> _clients;

		public ClientRepository(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);
			_clients = database.GetCollection<Client>(settings.ClientsCollectionName);
		}

		public async Task CreateAsync(Client client) =>
			await _clients.InsertOneAsync(client);

		public async Task<IEnumerable<Client>> GetAsync() =>
			await _clients.Find(_ => true).ToListAsync();

		public async Task<Client?> GetAsync(string id) =>
			await _clients.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task RemoveAsync(string id) =>
			await _clients.DeleteOneAsync(x => x.Id == id);

		public async Task UpdateAsync(string id, Client updatedClient) =>
			await _clients.ReplaceOneAsync(x => x.Id == id, updatedClient);

		
    }
}

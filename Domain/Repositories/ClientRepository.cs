using Domain.Entities;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace domain.Repositories
{
	public class ClientRepository : IClientRepository
	{
		private readonly IMongoCollection<Client> _clients;

		public ClientRepository(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);
			_clients = database.GetCollection<Client>(settings.ClientsCollectionName);
			
			var indexKeysDefinition = Builders<Client>
				.IndexKeys.Descending(x => x.LastUpdatedAt);
			_clients.Indexes.CreateOneAsync(new CreateIndexModel<Client>(indexKeysDefinition));
		}

		public async Task CreateAsync(Client client) =>
			await _clients.InsertOneAsync(client);

		public async Task<IEnumerable<Client>> GetAsync(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
		{
			if (lastUpdatedAt != null)
			{
				var filter = Builders<Client>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				if (isDeleted == false)
				{
					filter &= Builders<Client>.Filter.Eq(x => x.IsDeleted , false);
				}
				return await _clients.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
			else
			{
				if (isDeleted == null || isDeleted == true)
				{
					return await _clients.Find(_ => true).SortByDescending(c => c.LastUpdatedAt).ToListAsync();	
				}
				else
				{
					return await _clients.Find(x => x.IsDeleted == false).SortByDescending(c => c.LastUpdatedAt).ToListAsync();
				}	
			}
		}

		public async Task<Client?> GetAsync(string id) =>
			await _clients.Find(x => x.Id == id).FirstOrDefaultAsync();
		
		public async Task<IEnumerable<Client>> SearchAsync(string search)
		{
			var filter = Builders<Client>.Filter.Empty;
			if (!string.IsNullOrEmpty((search)))
			{
				filter = Builders<Client>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
			}
			return await _clients.Find(filter).SortByDescending(c => c.LastUpdatedAt).ToListAsync();
		}

		public async Task RemoveAsync(string id, string? usernameActor) =>
			await _clients.UpdateOneAsync(b => b.Id == id,
				Builders<Client>.Update
					.Set(b => b.IsDeleted, true)
					.Set(b => b.LastUpdatedAt,DateTimeOffset.UtcNow));

		public async Task UpdateAsync(string id, Client updatedClient) =>
			await _clients.ReplaceOneAsync(x => x.Id == id, updatedClient);
    }
}

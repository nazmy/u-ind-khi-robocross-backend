using Domain.Entities;
using Domain.Helper;
using MongoDB.Bson;
using MongoDB.Driver;

namespace domain.Repositories
{
	public class CompoundRepository : ICompoundRepository
	{
		private readonly IMongoCollection<Compound> _compound;

		public CompoundRepository(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);
			_compound = database.GetCollection<Compound>(settings.CompoundsCollectionName);
			
			var indexKeysDefinition = Builders<Compound>
				.IndexKeys.Descending(x => x.LastUpdatedAt);
			_compound.Indexes.CreateOneAsync(new CreateIndexModel<Compound>(indexKeysDefinition));
		}

		public async Task CreateAsync(Compound compound) =>
			await _compound.InsertOneAsync(compound);

		public async Task<IEnumerable<Compound>> GetAsync(DateTimeOffset? lastUpdatedAt)
		{
			if (lastUpdatedAt != null)
			{
				var filter = Builders<Compound>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				return await _compound.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
			else
			{
				return await _compound.Find(_ => true).SortByDescending(c => c.LastUpdatedAt).ToListAsync();	
			}
		}

		public async Task<Compound?> GetAsync(string id) =>
			await _compound.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task<IEnumerable<Compound>> SearchAsync(string search)
		{
			var filter = Builders<Compound>.Filter.Empty;
			if (!string.IsNullOrEmpty((search)))
			{
				filter = Builders<Compound>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
			}
			return await _compound.Find(filter).SortByDescending(c => c.LastUpdatedAt).ToListAsync();
		}

		public async Task<IEnumerable<Compound>> GetAsyncByClientId(string clientId, DateTimeOffset? lastUpdatedAt)
		{
			if (lastUpdatedAt != null)
			{
				var filter = Builders<Compound>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				filter &= Builders<Compound>.Filter.Eq(x => x.ClientId, clientId);
				return await _compound.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
			}
			else
			{
				return await _compound.Find(x => x.ClientId == clientId)
					.SortByDescending(c => c.LastUpdatedAt)
                				.ToListAsync();
			}
		}

		public async Task RemoveAsync(string id, string? usernameActor) =>
			await _compound.UpdateOneAsync(b => b.Id == id,
				Builders<Compound>.Update
					.Set(b => b.IsDeleted, true)
					.Set(b => b.LastUpdatedAt,DateTimeOffset.UtcNow)
				);

		public async Task UpdateAsync(string id, Compound updatedCompound) =>
			await _compound.ReplaceOneAsync(x => x.Id == id, updatedCompound);
    }
}

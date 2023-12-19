using Domain.Entities;
using Domain.Helper;
using domain.Identity;
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
		
		private FilterDefinition<Compound> GetFilterByLoggedInUser(LoggedInUser loggedInUser)
		{
			FilterDefinition<Compound> filter = FilterDefinition<Compound>.Empty;
			if (loggedInUser.Role == "Customer")
			{
				filter = Builders<Compound>.Filter.Eq(x => x.ClientId, loggedInUser.ClientId);	
			}

			return filter;
		} 

		public async Task<IEnumerable<Compound>> GetAsync(LoggedInUser loggedInUser, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
		{
			var filter = GetFilterByLoggedInUser(loggedInUser);
			
			if (lastUpdatedAt != null)
			{
				filter &= Builders<Compound>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				if (isDeleted == false)
				{
					filter &= Builders<Compound>.Filter.Eq(x => x.IsDeleted , false);
				}
				return await _compound.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
			else
			{
				if (isDeleted == null || isDeleted == true)
				{
					return await _compound.Find(filter).SortByDescending(c => c.LastUpdatedAt).ToListAsync();	
				}
				else
				{
					filter &= Builders<Compound>.Filter.Eq(x => x.IsDeleted , false);
					return await _compound.Find(filter).SortByDescending(c => c.LastUpdatedAt).ToListAsync();
				}	
			}
		}

		public async Task<Compound?> GetAsync(LoggedInUser loggedInUser, string id)
		{
			var filter = GetFilterByLoggedInUser(loggedInUser);
			filter &= Builders<Compound>.Filter.Eq(x => x.Id, id);
			return await _compound.Find(filter).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Compound>> SearchAsync(LoggedInUser loggedInUser, string search)
		{
			var filter = GetFilterByLoggedInUser(loggedInUser);
			
			if (!string.IsNullOrEmpty((search)))
			{
				filter &= Builders<Compound>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
			}
			return await _compound.Find(filter).SortByDescending(c => c.LastUpdatedAt).ToListAsync();
		}

		public async Task<IEnumerable<Compound>> GetAsyncByClientId(LoggedInUser loggedInUser, string clientId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
		{
			var filter = GetFilterByLoggedInUser(loggedInUser);
			if (lastUpdatedAt != null)
			{
				filter &= Builders<Compound>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				filter &= Builders<Compound>.Filter.Eq(x => x.ClientId, clientId);
				if (isDeleted == false)
				{
					filter &= Builders<Compound>.Filter.Eq(x => x.IsDeleted , false); 
				}
				return await _compound.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
			}
			else
			{
				filter &= Builders<Compound>.Filter.Eq(x => x.ClientId,clientId);
				if (isDeleted == null || isDeleted == true)
				{
					filter &= Builders<Compound>.Filter.Eq(x => x.IsDeleted , false);
				}
				return await _compound.Find(filter)
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

using Domain.Entities;
using Domain.Helper;
using domain.Identity;
using MongoDB.Bson;
using MongoDB.Driver;

namespace domain.Repositories
{
	public class LineRepository : ILineRepository
	{
		private readonly IMongoCollection<Line> _line;

		public LineRepository(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);
			_line = database.GetCollection<Line>(settings.LinesCollectionName);
			
			var indexKeysDefinition = Builders<Line>
				.IndexKeys.Descending(x => x.LastUpdatedAt);
			_line.Indexes.CreateOneAsync(new CreateIndexModel<Line>(indexKeysDefinition));
		}

		public async Task CreateAsync(Line line) =>
			await _line.InsertOneAsync(line);
		
		private FilterDefinition<Line> GetFilterByLoggedInUser(LoggedInUser loggedInUser)
		{
			FilterDefinition<Line> filter = FilterDefinition<Line>.Empty;
			if (loggedInUser.Role == "Customer")
			{
				filter = Builders<Line>.Filter.Eq(x => x.ClientId, loggedInUser.ClientId);	
			}

			return filter;
		} 

		public async Task<IEnumerable<Line>> GetAsync(LoggedInUser loggedInUser, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
		{
			var filter = GetFilterByLoggedInUser(loggedInUser);
			
			if (lastUpdatedAt != null)
			{
				filter &= Builders<Line>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				if (isDeleted == false)
				{
					filter &= Builders<Line>.Filter.Eq(x => x.IsDeleted , false);
				}
				return await _line.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
			else
			{
				if (isDeleted == null || isDeleted == true)
				{
					return await _line.Find(filter).SortByDescending(l => l.LastUpdatedAt).ToListAsync();
				}
				else
				{
					filter &= Builders<Line>.Filter.Eq(x => x.IsDeleted , false);
					return await _line.Find(filter).SortByDescending(l => l.LastUpdatedAt).ToListAsync();
				}	
			}
		}

		public async Task<Line> GetAsync(LoggedInUser loggedInUser, string id)
		{
			var filter = GetFilterByLoggedInUser(loggedInUser);
			filter &= Builders<Line>.Filter.Eq(x => x.Id, id);
			return await _line.Find(filter).SortByDescending(l => l.LastUpdatedAt).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Line>> SearchAsync(LoggedInUser loggedInUser, string search)
		{
			var filter = GetFilterByLoggedInUser(loggedInUser);
			
			if (!string.IsNullOrEmpty((search)))
			{
				filter &= Builders<Line>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
			}
			return await _line.Find(filter).SortByDescending(l  => l.LastUpdatedAt).ToListAsync();
		}

		public async Task<IEnumerable<Line>> GetAsyncByBuildingId(LoggedInUser loggedInUser, string buildingId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
		{
			var filter = GetFilterByLoggedInUser(loggedInUser);
			if (lastUpdatedAt != null)
			{
				filter &= Builders<Line>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				filter &= Builders<Line>.Filter.Eq(x => x.BuildingId, buildingId);
				if (isDeleted == false)
				{
					filter &= Builders<Line>.Filter.Eq(x => x.IsDeleted , false); 
				}
				return await _line.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
			}
			else
			{
				filter &= Builders<Line>.Filter.Eq(x => x.BuildingId, buildingId);
				if (isDeleted == null || isDeleted == true)
				{
					filter &= Builders<Line>.Filter.Eq(x => x.IsDeleted , false);
				}
				return await _line.Find(filter).SortByDescending(l => l.LastUpdatedAt)
                				.ToListAsync();
			}
			
		}

		public async Task<IEnumerable<Line>> GetAsyncByIntegratorId(LoggedInUser loggedInUser, string integratorId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
		{
			var filter = GetFilterByLoggedInUser(loggedInUser);
			if (lastUpdatedAt != null)
			{
				filter &= Builders<Line>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				filter &= Builders<Line>.Filter.Eq(x => x.IntegratorId, integratorId);
				if (isDeleted == false)
				{
					filter &= Builders<Line>.Filter.Eq(x => x.IsDeleted , false); 
				}
				return await _line.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
			}
			else
			{
				filter &= Builders<Line>.Filter.Eq(x => x.IntegratorId, integratorId);
				if (isDeleted == null || isDeleted == true)
				{
					filter &= Builders<Line>.Filter.Eq(x => x.IsDeleted , false);
				}
				return await _line.Find(filter).SortByDescending(l => l.LastUpdatedAt)
					.ToListAsync();
			}
		}

		public async Task RemoveAsync(string id, string? usernameActor) =>
			await _line.UpdateOneAsync(line => line.Id == id,
				Builders<Line>.Update
					.Set(line => line.IsDeleted, true)
					.Set(line => line.LastUpdatedAt,DateTimeOffset.UtcNow));

		public async Task UpdateAsync(string id, Line updatedLine) =>
			await _line.ReplaceOneAsync(x => x.Id == id, updatedLine);
    }
}

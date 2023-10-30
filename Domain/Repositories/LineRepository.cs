using Domain.Entities;
using Domain.Helper;
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

		public async Task<IEnumerable<Line>> GetAsync(DateTimeOffset? lastUpdatedAt)
		{
			if (lastUpdatedAt != null)
			{
				var filter = Builders<Line>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				return await _line.Find(filter).SortByDescending(x => x.LastUpdatedAt).ToListAsync();
			}
			else
			{
				return await _line.Find(_ => true).SortByDescending(l => l.LastUpdatedAt).ToListAsync();	
			}
		}

		public async Task<Line> GetAsync(string id) =>
			await _line.Find(x => x.Id == id).SortByDescending(l => l.LastUpdatedAt).FirstOrDefaultAsync();

		public async Task<IEnumerable<Line>> SearchAsync(string search)
		{
			var filter = Builders<Line>.Filter.Empty;
			if (!string.IsNullOrEmpty((search)))
			{
				filter = Builders<Line>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
			}
			return await _line.Find(filter).SortByDescending(l  => l.LastUpdatedAt).ToListAsync();
		}

		public async Task<IEnumerable<Line>> GetAsyncByBuildingId(string buildingId, DateTimeOffset? lastUpdatedAt)
		{
			if (lastUpdatedAt != null)
			{
				var filter = Builders<Line>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				filter &= Builders<Line>.Filter.Eq(x => x.BuildingId, buildingId);
				return await _line.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
			}
			else
			{
				return await _line.Find(x => x.BuildingId == buildingId).SortByDescending(l => l.LastUpdatedAt)
                				.ToListAsync();
			}
			
		}

		public async Task<IEnumerable<Line>> GetAsyncByIntegratorId(string integratorId, DateTimeOffset? lastUpdatedAt)
		{
			if (lastUpdatedAt != null)
			{
				var filter = Builders<Line>.Filter.Gte("LastUpdatedAt.0", lastUpdatedAt.Value.Ticks);
				filter &= Builders<Line>.Filter.Eq(x => x.IntegratorId, integratorId);
				return await _line.Find(filter).SortByDescending(b => b.LastUpdatedAt).ToListAsync();
			}
			else
			{
				return await _line.Find(x => x.IntegratorId == integratorId).SortByDescending(l => l.LastUpdatedAt)
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

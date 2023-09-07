using System;
using Domain.Entities;
using Domain.Helper;
using MongoDB.Driver;
using Domain.Dto;
using MongoDB.Bson;

namespace khi_robocross_api.Services
{
	public class LineRepository : ILineRepository
	{
		private readonly IMongoCollection<Line> _line;

		public LineRepository(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);
			_line = database.GetCollection<Line>(settings.LinesCollectionName);
		}

		public async Task CreateAsync(Line line) =>
			await _line.InsertOneAsync(line);

		public async Task<IEnumerable<Line>> GetAsync() =>
			await _line.Find(_ => true).ToListAsync();

		public async Task<Line> GetAsync(string id) =>
			await _line.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task<IEnumerable<Line>> SearchAsync(string search)
		{
			var filter = Builders<Line>.Filter.Empty;
			if (!string.IsNullOrEmpty((search)))
			{
				filter = Builders<Line>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
			}
			return await _line.Find(filter).ToListAsync();
		}

		public async Task<IEnumerable<Line>> GetAsyncByBuildingId(string buildingId) =>
            await _line.Find(x => x.BuildingId == buildingId).ToListAsync();

		public async Task<IEnumerable<Line>> GetAsyncByIntegratorId(string integratorId) =>
			await _line.Find(x => x.IntegratorId == integratorId).ToListAsync();
		
        public async Task RemoveAsync(string id) =>
			await _line.DeleteOneAsync(x => x.Id == id);

		public async Task UpdateAsync(string id, Line updatedLine) =>
			await _line.ReplaceOneAsync(x => x.Id == id, updatedLine);
    }
}

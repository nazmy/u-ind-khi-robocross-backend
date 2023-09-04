﻿using System;
using Domain.Entities;
using Domain.Helper;
using MongoDB.Driver;
using Domain.Dto;
using MongoDB.Bson;

namespace khi_robocross_api.Services
{
	public class CompoundRepository : ICompoundRepository
	{
		private readonly IMongoCollection<Compound> _compound;

		public CompoundRepository(IRobocrossDatabaseSettings settings, IMongoClient mongoClient)
		{
			var database = mongoClient.GetDatabase(settings.DatabaseName);
			_compound = database.GetCollection<Compound>(settings.CompoundsCollectionName);
		}

		public async Task CreateAsync(Compound compound) =>
			await _compound.InsertOneAsync(compound);

		public async Task<IEnumerable<Compound>> GetAsync() =>
			await _compound.Find(_ => true).ToListAsync();

		public async Task<Compound?> GetAsync(string id) =>
			await _compound.Find(x => x.Id == id).FirstOrDefaultAsync();

		public async Task<IEnumerable<Compound>> SearchAsync(string search)
		{
			var filter = Builders<Compound>.Filter.Empty;
			if (!string.IsNullOrEmpty((search)))
			{
				filter = Builders<Compound>.Filter.Regex("Name", new BsonRegularExpression(search, "i"));
			}
			return await _compound.Find(filter).ToListAsync();
		}

		public async Task<IEnumerable<Compound>> GetAsyncByClientId(string clientId) =>
            await _compound.Find(x => x.Client.Id == clientId).ToListAsync();

        public async Task RemoveAsync(string id) =>
			await _compound.DeleteOneAsync(x => x.Id == id);

		public async Task UpdateAsync(string id, Compound updatedCompound) =>
			await _compound.ReplaceOneAsync(x => x.Id == id, updatedCompound);
    }
}

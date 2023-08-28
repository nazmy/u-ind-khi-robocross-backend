using System;
using Domain.Entities;
using MongoDB.Driver;
using Domain.Dto;

namespace khi_robocross_api.Services
{
	public interface ICompoundRepository
	{
		Task<IEnumerable<Compound>> GetAsync();
		Task<Compound> GetAsync(String id);
		Task CreateAsync(Compound compound);
		Task UpdateAsync(string id, Compound updatedCompound);
		Task RemoveAsync(string id);
        Task<IEnumerable<Compound>> GetAsyncByClientId(string clientId);
    }
}


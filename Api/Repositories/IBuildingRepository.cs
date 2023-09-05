using System;
using Domain.Entities;
using MongoDB.Driver;
using Domain.Dto;

namespace khi_robocross_api.Services
{
	public interface IBuildingRepository
	{
		Task<IEnumerable<Building>> GetAsync();
		Task<Building> GetAsync(string id);
		Task<IEnumerable<Building>> SearchAsync(string search);
		Task CreateAsync(Building building);
		Task UpdateAsync(string id, Building updatedBuilding);
		Task RemoveAsync(string id);
        Task<IEnumerable<Building>> GetAsyncByCompoundId(string compoundId);
    }
}


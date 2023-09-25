using System;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public interface IBuildingService
	{
		 ValueTask<IEnumerable<BuildingResponse>> GetAllBuildings();
		 ValueTask<BuildingResponse> GetBuildingById(String id);
         ValueTask<IEnumerable<BuildingResponse>> GetBuildingByCompoundId(String compoundId);
         Task AddBuilding(Building building);
         Task UpdateBuilding(string id, UpdateBuildingInput updatedBuilding);
         Task RemoveBuilding(string id);
	}
}


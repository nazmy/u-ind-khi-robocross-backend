using System;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public interface ILineService
	{
		 ValueTask<IEnumerable<LineResponse>> GetAllLines();
		 ValueTask<LineResponse> GetLineById(String id);
         ValueTask<IEnumerable<LineResponse>> GetLineByBuildingId(String buildingId);
         ValueTask<IEnumerable<LineResponse>> GetLineByIntegratorId(String integratorId);
         Task AddLine(Line line);
         Task UpdateLine(string id, UpdateLineInput updatedLine);
         Task RemoveLine(string id);
	}
}


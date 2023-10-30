using System;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public interface ILineService
	{
		 ValueTask<IEnumerable<LineResponse>> GetAllLines(DateTimeOffset? lastUpdatedAt);
		 ValueTask<LineResponse> GetLineById(String id);
         ValueTask<IEnumerable<LineResponse>> GetLineByBuildingId(String buildingId, DateTimeOffset? lastUpdatedAt);
         ValueTask<IEnumerable<LineResponse>> GetLineByIntegratorId(String integratorId, DateTimeOffset? lastUpdatedAt);
         Task AddLine(Line line);
         Task UpdateLine(string id, UpdateLineInput updatedLine);
         Task RemoveLine(string id);
	}
}


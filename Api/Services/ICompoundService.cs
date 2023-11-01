using System;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public interface ICompoundService
	{
		 ValueTask<IEnumerable<CompoundResponse>> GetAllCompounds(DateTimeOffset? lastUpdatedAt, bool? isDeleted);
		 ValueTask<CompoundResponse> GetCompoundById(String id);
         ValueTask<IEnumerable<CompoundResponse>> GetCompoundByClientId(String clientId, DateTimeOffset? lastUpdatedAt, bool? isDeleted);
         Task AddCompound(Compound inputCompound);
         Task UpdateCompound(string id, UpdateCompoundInput updateCompoundInput);
         Task RemoveCompound(string id);
	}
}


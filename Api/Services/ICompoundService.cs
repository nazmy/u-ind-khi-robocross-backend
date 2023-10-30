﻿using System;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public interface ICompoundService
	{
		 ValueTask<IEnumerable<CompoundResponse>> GetAllCompounds(DateTimeOffset? lastUpdatedAt);
		 ValueTask<CompoundResponse> GetCompoundById(String id);
         ValueTask<IEnumerable<CompoundResponse>> GetCompoundByClientId(String clientId, DateTimeOffset? lastUpdatedAt);
         Task AddCompound(Compound inputCompound);
         Task UpdateCompound(string id, UpdateCompoundInput updateCompoundInput);
         Task RemoveCompound(string id);
	}
}


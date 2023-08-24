using System;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public interface ICompoundService
	{
		 ValueTask<IEnumerable<CompoundOutputDto>> GetAllCompounds();
		 ValueTask<CompoundOutputDto> GetCompoundById(String id);
		 Task AddCompound(Compound compound);
         Task UpdateCompound(string id, UpdateCompoundInputDto updateCompoundInput);
         Task RemoveCompound(string id);
	}
}


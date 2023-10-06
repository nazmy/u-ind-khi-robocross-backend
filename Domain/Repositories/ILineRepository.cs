using Domain.Entities;

namespace domain.Repositories
{
	public interface ILineRepository
	{
		Task<IEnumerable<Line>> GetAsync();
		Task<Line> GetAsync(string id);
		Task<IEnumerable<Line>> SearchAsync(string search);
		Task CreateAsync(Line line);
		Task UpdateAsync(string id, Line updatedLine);
		Task RemoveAsync(string id);
        Task<IEnumerable<Line>> GetAsyncByBuildingId(string buildingId);
        Task<IEnumerable<Line>> GetAsyncByIntegratorId(string integratorId);
    }
}


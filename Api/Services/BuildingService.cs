using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using domain.Repositories;
using domain.Identity;
using domain.Identity.Manager;
using domain.Repositories;
using domain.Repositories.Manager;
using Microsoft.AspNetCore.Authentication;

namespace khi_robocross_api.Services
{
	public class BuildingService : IBuildingService
	{
        private readonly IBuildingRepository _buildingRepository;
        private readonly ICompoundRepository _compoundRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public BuildingService(IBuildingRepository buildingRepository,
            ICompoundRepository compoundRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
		{
            _buildingRepository = buildingRepository;
            _compoundRepository = compoundRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddBuilding(Building inputBuilding)
        {
            if (inputBuilding == null)
                throw new ArgumentException("Building input is invalid");

            var compound = await _compoundRepository.GetAsync(inputBuilding.CompoundId);
            inputBuilding.CompoundId = compound?.Id;
            inputBuilding.ClientId = compound?.ClientId;
            inputBuilding.CreateChangesTime(inputBuilding, _httpContextAccessor.HttpContext.User.Identity.Name);
            
            //validation goes here
            await _buildingRepository.CreateAsync(inputBuilding);
        }

        public async ValueTask<IEnumerable<BuildingResponse>> GetAllBuildings(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {

            var buildingTask = await _buildingRepository.GetAsync(lastUpdatedAt,isDeleted);
            
            return _mapper.Map<IEnumerable<BuildingResponse>>(buildingTask.ToList());
        }

        public async ValueTask<IEnumerable<BuildingResponse>> GetBuildingByCompoundId(string compoundId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            var buildingTask = await _buildingRepository.GetAsyncByCompoundId(compoundId, lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<BuildingResponse>>(buildingTask.ToList());
        }

        public async ValueTask<BuildingResponse> GetBuildingById(string id)
        {
            if (id == null)
                throw new ArgumentException("Building Id is Invalid");

            var buildingTask = await _buildingRepository.GetAsync(id);
            return _mapper.Map<BuildingResponse>(buildingTask);
        }

        public async Task RemoveBuilding(string id)
        {
            if (id == null)
                throw new ArgumentException("Building Id is Invalid");

            await _buildingRepository.RemoveAsync(id,_httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public async Task UpdateBuilding(string id, UpdateBuildingInput updatedBuilding)
        {
            if (id == null)
                throw new ArgumentException("Building Id is invalid");

            if (updatedBuilding == null)
                throw new ArgumentException("Building Input is invalid");
            
            var building = await _buildingRepository.GetAsync(id);

            if (building == null)
                throw new KeyNotFoundException($"Building with Id = {id} not found");

            _mapper.Map<UpdateBuildingInput, Building>(updatedBuilding, building);
            building.UpdateChangesTime(building, _httpContextAccessor.HttpContext.User.Identity.Name);
            await _buildingRepository.UpdateAsync(id, building);
        }
    }
}


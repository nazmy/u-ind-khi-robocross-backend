using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using domain.Identity;
using domain.Repositories;
using khi_robocross_api.Helper;

namespace khi_robocross_api.Services
{
	public class BuildingService : IBuildingService
	{
        private readonly IBuildingRepository _buildingRepository;
        private readonly ICompoundRepository _compoundRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggedInUser _loggedInUser;

        public BuildingService(IBuildingRepository buildingRepository,
            ICompoundRepository compoundRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILoggedInUser loggedInUser
            )
		{
            _buildingRepository = buildingRepository;
            _compoundRepository = compoundRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = loggedInUser;
        }

        public async Task AddBuilding(Building inputBuilding)
        {
            if (inputBuilding == null)
                throw new ArgumentException("Building input is invalid");
            
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            
            var compound = await _compoundRepository.GetAsync(loggedInUser, inputBuilding.CompoundId);
            
            inputBuilding.CompoundId = compound?.Id;
            inputBuilding.ClientId = compound?.ClientId;
            
            inputBuilding.CreateChangesTime(inputBuilding, IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
            
            //validation goes here
            await _buildingRepository.CreateAsync(inputBuilding);
        }

        public async ValueTask<IEnumerable<BuildingResponse>> GetAllBuildings(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {

            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var buildingTask = await _buildingRepository.GetAsync(loggedInUser,lastUpdatedAt,isDeleted);
            return _mapper.Map<IEnumerable<BuildingResponse>>(buildingTask.ToList());
        }

        public async ValueTask<IEnumerable<BuildingResponse>> GetBuildingByCompoundId(string compoundId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var buildingTask = await _buildingRepository.GetAsyncByCompoundId(loggedInUser,compoundId, lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<BuildingResponse>>(buildingTask.ToList());
        }

        public async ValueTask<BuildingResponse> GetBuildingById(string id)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            if (id == null)
                throw new ArgumentException("Building Id is Invalid");

            var buildingTask = await _buildingRepository.GetAsync(loggedInUser, id);
            return _mapper.Map<BuildingResponse>(buildingTask);
        }
        
        public async ValueTask<IEnumerable<BuildingResponse>> Query(string search)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var buildingTask = await _buildingRepository.SearchAsync(loggedInUser,search);
            return _mapper.Map<IEnumerable<BuildingResponse>>(buildingTask.ToList());
        }

        public async Task RemoveBuilding(string id)
        {
            if (id == null)
                throw new ArgumentException("Building Id is Invalid");

            await _buildingRepository.RemoveAsync(id,IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
        }

        public async Task UpdateBuilding(string id, UpdateBuildingInput updatedBuilding)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            
            if (id == null)
                throw new ArgumentException("Building Id is invalid");

            if (updatedBuilding == null)
                throw new ArgumentException("Building Input is invalid");
            
            var building = await _buildingRepository.GetAsync(loggedInUser,id);

            if (building == null)
                throw new KeyNotFoundException($"Building with Id = {id} not found");

            _mapper.Map<UpdateBuildingInput, Building>(updatedBuilding, building);
            building.UpdateChangesTime(building, IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
            await _buildingRepository.UpdateAsync(id, building);
        }
    }
}


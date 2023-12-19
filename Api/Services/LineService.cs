using System;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using domain.Identity;
using domain.Repositories;
using khi_robocross_api.Helper;
using MongoDB.Bson;

namespace khi_robocross_api.Services
{
	public class LineService : ILineService
	{
        private readonly ILineRepository _lineRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggedInUser _loggedInUser;

		public LineService(ILineRepository lineRepository,
            IBuildingRepository buildingRepository,
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor,
            ILoggedInUser loggedInUser)
		{
            _lineRepository = lineRepository;
            _buildingRepository = buildingRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = loggedInUser;
        }

        public async Task AddLine(Line line)
        {
            if (line == null)
                throw new ArgumentException("Line input is invalid");
            
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            
            var building = await _buildingRepository.GetAsync(loggedInUser,line.BuildingId);
            line.ClientId = building?.ClientId;
            
            List<Unit> unitList = line.Units;
            foreach (Unit unit in unitList)
            {
                unit.Id = ObjectId.GenerateNewId().ToString();
                List<SceneObject> sceneObjectList = unit.SceneObjects;
                foreach (SceneObject sceneObject in sceneObjectList)
                {
                    sceneObject.Id = ObjectId.GenerateNewId().ToString();
                }
            }

            //TEMP: for create line, use the createdBy supply at the body payload
            line.CreateChangesTime(line, line.CreatedBy);
            
            //validation goes here
            await _lineRepository.CreateAsync(line);
        }

        public async ValueTask<IEnumerable<LineResponse>> GetAllLines(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var lineTask = await _lineRepository.GetAsync(loggedInUser,lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<LineResponse>>(lineTask.ToList());
        }

        public async ValueTask<IEnumerable<LineResponse>> GetLineByBuildingId(string buildingId,DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var lineTask = await _lineRepository.GetAsyncByBuildingId(loggedInUser, buildingId, lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<LineResponse>>(lineTask.ToList());
        }
        
        public async ValueTask<IEnumerable<LineResponse>> GetLineByIntegratorId(string integratorId,DateTimeOffset? lastUpdatedAt,  bool? isDeleted)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var lineTask = await _lineRepository.GetAsyncByIntegratorId(loggedInUser,integratorId,lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<LineResponse>>(lineTask.ToList());
        }

        public async ValueTask<LineResponse> GetLineById(string id)
        {
            if (id == null)
                throw new ArgumentException("Line Id is Invalid");
            
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            
            var lineTask = await _lineRepository.GetAsync(loggedInUser,id);
            return _mapper.Map<LineResponse>(lineTask);
        }
        
        public async ValueTask<IEnumerable<LineResponse>> Query(string search)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var lineTask = await _lineRepository.SearchAsync(loggedInUser,search);
            return _mapper.Map<IEnumerable<LineResponse>>(lineTask.ToList());
        }

        public async Task RemoveLine(string id)
        {
            if (id == null)
                throw new ArgumentException("Line Id is Invalid");

            await _lineRepository.RemoveAsync(id,IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
        }

        public async Task UpdateLine(string id, UpdateLineInput updatedLine)
        {
            if (id == null)
                throw new ArgumentException("Line Id is invalid");

            if (updatedLine == null)
                throw new ArgumentException("Line Input is invalid");
            
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var line = await _lineRepository.GetAsync(loggedInUser,id);

            if (line == null)
                throw new KeyNotFoundException($"Line with Id = {id} not found");

            _mapper.Map<UpdateLineInput, Line>(updatedLine, line);
            
            foreach (Unit unitItem in line.Units)
            {
                if (String.IsNullOrEmpty(unitItem.Id))
                    unitItem.Id = ObjectId.GenerateNewId().ToString();

                foreach (SceneObject sceneObjectItem in unitItem.SceneObjects)
                {
                    if (String.IsNullOrEmpty(sceneObjectItem.Id))
                        sceneObjectItem.Id = ObjectId.GenerateNewId().ToString();
                }
            }
            line.UpdateChangesTime(line, IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
            await _lineRepository.UpdateAsync(id, line);
        }
    }
}


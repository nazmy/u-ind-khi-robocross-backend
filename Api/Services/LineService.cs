using System;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using domain.Repositories;
using MongoDB.Bson;

namespace khi_robocross_api.Services
{
	public class LineService : ILineService
	{
        private readonly ILineRepository _lineRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

		public LineService(ILineRepository lineRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
            _lineRepository = lineRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddLine(Line line)
        {
            if (line == null)
                throw new ArgumentException("Line input is invalid");

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

            line.CreateChangesTime(line, _httpContextAccessor.HttpContext.User.Identity.Name);
            
            //validation goes here
            await _lineRepository.CreateAsync(line);
        }

        public async ValueTask<IEnumerable<LineResponse>> GetAllLines(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            var lineTask = await _lineRepository.GetAsync(lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<LineResponse>>(lineTask.ToList());
        }

        public async ValueTask<IEnumerable<LineResponse>> GetLineByBuildingId(string buildingId,DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            var lineTask = await _lineRepository.GetAsyncByBuildingId(buildingId,lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<LineResponse>>(lineTask.ToList());
        }
        
        public async ValueTask<IEnumerable<LineResponse>> GetLineByIntegratorId(string integratorId,DateTimeOffset? lastUpdatedAt,  bool? isDeleted)
        {
            var lineTask = await _lineRepository.GetAsyncByIntegratorId(integratorId,lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<LineResponse>>(lineTask.ToList());
        }

        public async ValueTask<LineResponse> GetLineById(string id)
        {
            if (id == null)
                throw new ArgumentException("Line Id is Invalid");

            var lineTask = await _lineRepository.GetAsync(id);
            return _mapper.Map<LineResponse>(lineTask);
        }

        public async Task RemoveLine(string id)
        {
            if (id == null)
                throw new ArgumentException("Line Id is Invalid");

            await _lineRepository.RemoveAsync(id,_httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public async Task UpdateLine(string id, UpdateLineInput updatedLine)
        {
            if (id == null)
                throw new ArgumentException("Line Id is invalid");

            if (updatedLine == null)
                throw new ArgumentException("Line Input is invalid");
            
            var line = await _lineRepository.GetAsync(id);

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
            line.UpdateChangesTime(line, _httpContextAccessor.HttpContext.User.Identity.Name);
            await _lineRepository.UpdateAsync(id, line);
        }
    }
}


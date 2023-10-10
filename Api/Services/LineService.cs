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

		public LineService(ILineRepository lineRepository, IMapper mapper)
		{
            _lineRepository = lineRepository;
            _mapper = mapper;
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
                    List<Robot> robotList = sceneObject.robots;
                    foreach (Robot robot in robotList)
                    {
                        robot.Id = ObjectId.GenerateNewId().ToString();
                    }
                }
            }

            line.CreateChangesTime(line);
            
            //validation goes here
            await _lineRepository.CreateAsync(line);
        }

        public async ValueTask<IEnumerable<LineResponse>> GetAllLines()
        {
            var lineTask = await _lineRepository.GetAsync();
            
            if (lineTask != null)
                return _mapper.Map<IEnumerable<LineResponse>>(lineTask.ToList());

            return null;
        }

        public async ValueTask<IEnumerable<LineResponse>> GetLineByBuildingId(string buildingId)
        {
            var lineTask = await _lineRepository.GetAsyncByBuildingId(buildingId);
            if (lineTask != null)
                return _mapper.Map<IEnumerable<LineResponse>>(lineTask.ToList());

            return null;
        }
        
        public async ValueTask<IEnumerable<LineResponse>> GetLineByIntegratorId(string integratorId)
        {
            var lineTask = await _lineRepository.GetAsyncByIntegratorId(integratorId);
            if (lineTask != null)
                return _mapper.Map<IEnumerable<LineResponse>>(lineTask.ToList());

            return null;
        }

        public async ValueTask<LineResponse> GetLineById(string id)
        {
            if (id == null)
                throw new ArgumentException("Line Id is Invalid");

            var lineTask = await _lineRepository.GetAsync(id);
            if (lineTask != null)
                return _mapper.Map<LineResponse>(lineTask);

            return null;
        }

        public async Task RemoveLine(string id)
        {
            if (id == null)
                throw new ArgumentException("Line Id is Invalid");

            await _lineRepository.RemoveAsync(id);
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

            line = _mapper.Map<Line>(updatedLine);
            
            foreach (Unit unitItem in line.Units)
            {
                if (String.IsNullOrEmpty(unitItem.Id))
                    unitItem.Id = ObjectId.GenerateNewId().ToString();

                foreach (SceneObject sceneObjectItem in unitItem.SceneObjects)
                {
                    if (String.IsNullOrEmpty(sceneObjectItem.Id))
                        sceneObjectItem.Id = ObjectId.GenerateNewId().ToString();

                    foreach (Robot robotItem in sceneObjectItem.robots)
                    {
                        if (String.IsNullOrEmpty(robotItem.Id))
                            robotItem.Id = ObjectId.GenerateNewId().ToString();
                    }
                }
            }
            line.UpdateChangesTime(line);
            
            await _lineRepository.UpdateAsync(id, line);
        }
    }
}


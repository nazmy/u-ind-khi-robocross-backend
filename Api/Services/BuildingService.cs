﻿using System;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public class BuildingService : IBuildingService
	{
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;

		public BuildingService(IBuildingRepository buildingRepository, IMapper mapper)
		{
            _buildingRepository = buildingRepository;
            _mapper = mapper;
        }

        public async Task AddBuilding(Building inputBuilding)
        {
            if (inputBuilding == null)
                throw new ArgumentException("Building input is invalid");

            inputBuilding.CreateChangesTime(inputBuilding);
            
            //validation goes here
            await _buildingRepository.CreateAsync(inputBuilding);
        }

        public async ValueTask<IEnumerable<BuildingResponse>> GetAllBuildings()
        {
            var buildingTask = await _buildingRepository.GetAsync();
            
            if (buildingTask != null)
                return _mapper.Map<IEnumerable<BuildingResponse>>(buildingTask.ToList());

            return null;
        }

        public async ValueTask<IEnumerable<BuildingResponse>> GetBuildingByCompoundId(string compoundId)
        {
            var buildingTask = await _buildingRepository.GetAsyncByCompoundId(compoundId);
            if (buildingTask != null)
                return _mapper.Map<IEnumerable<BuildingResponse>>(buildingTask.ToList());

            return null;
        }

        public async ValueTask<BuildingResponse> GetBuildingById(string id)
        {
            if (id == null)
                throw new ArgumentException("Building Id is Invalid");

            var buildingTask = await _buildingRepository.GetAsync(id);
            if (buildingTask != null)
                return _mapper.Map<BuildingResponse>(buildingTask);

            return null;
        }

        public async Task RemoveBuilding(string id)
        {
            if (id == null)
                throw new ArgumentException("Building Id is Invalid");

            await _buildingRepository.RemoveAsync(id);
        }

        public async Task UpdateBuilding(string id, UpdateBuildingInputDto updatedBuilding)
        {
            if (id == null)
                throw new ArgumentException("Building Id is invalid");

            if (updatedBuilding == null)
                throw new ArgumentException("Building Input is invalid");
            
            var building = await _buildingRepository.GetAsync(id);

            if (building == null)
                throw new KeyNotFoundException($"Building with Id = {id} not found");

            building = _mapper.Map<Building>(updatedBuilding);
            building.UpdateChangesTime(building);
            
            await _buildingRepository.UpdateAsync(id, building);
        }
    }
}

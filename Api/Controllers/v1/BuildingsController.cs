using System;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using GeoJSON.Net.Geometry;
using khi_robocross_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace khi_robocross_api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class BuildingsController : ControllerBase
	{
        private readonly IBuildingService _buildingService;
        private readonly ILineService _lineService;
        private readonly IMapper _mapper;

        public BuildingsController(IBuildingService buildingService,
            ILineService lineService,
            IMapper mapper)
        {
            _buildingService = buildingService;
            _lineService = lineService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(DateTimeOffset? lastUpdatedAt)
        {
            var buildingList = await _buildingService.GetAllBuildings(lastUpdatedAt);
            return Ok(buildingList);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<BuildingResponse>> Get(string id)
        {
            try
            {
                var building = await _buildingService.GetBuildingById(id);
                if (building == null)
                {
                    return NotFound($"Building with Id = {id} not found");
                }

                return Ok(building);
            }
            catch (ArgumentException aex)
            {
                return BadRequest("Invalid Building ID");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CreateBuildingInput newBuilding)
        {
            if (newBuilding == null)
                return BadRequest(ModelState);
            
            var building = _mapper.Map<Building>(newBuilding);
            await _buildingService.AddBuilding(building);

            return CreatedAtAction(nameof(Get), new { id = building.Id }, building);
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, UpdateBuildingInput updatedBuilding)
        {
            try
            {
                if (updatedBuilding == null)
                    return BadRequest(ModelState);

                await _buildingService.UpdateBuilding(id, updatedBuilding);
                return NoContent();
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (KeyNotFoundException kex)
            {
                return NotFound(kex.Message);
            }
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            var building = await _buildingService.GetBuildingById(id);

            if (building is null)
            {
                return NotFound($"Building with Id = {id} not found");
            }

            await _buildingService.RemoveBuilding(id);
            return Ok($"Building with Id = {id} deleted");
        }
        
        //Get Lines of Business
        [HttpGet("{id}/Lines")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBuildingLines(string id, DateTimeOffset? lastUpdatedAt)
        {
            var lineList = await _lineService.GetLineByBuildingId(id,lastUpdatedAt);
            return Ok(lineList);
        }
    }
}


﻿using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using khi_robocross_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace khi_robocross_api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class LinesController : ControllerBase
	{
        private readonly ILineService _lineService;
        private readonly IMapper _mapper;
        private readonly ILogger<LinesController> _logger;

        public LinesController(ILineService lineService,
            IMapper mapper,
            ILogger<LinesController> logger)
        {
            _lineService = lineService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            var lineList = await _lineService.GetAllLines(lastUpdatedAt, isDeleted);
            return Ok(lineList);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<LineResponse>> Get(string id)
        {
            try
            {
                var line = await _lineService.GetLineById(id);
                if (line == null)
                {
                    return NotFound($"Line with Id = {id} not found");
                }

                return Ok(line);
            }
            catch (ArgumentException aex)
            {
                return BadRequest("Invalid Line ID");
            }
        }
        
        [HttpGet("Search")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LineResponse>))]
        public async Task<ActionResult<LineResponse>> Search([FromQuery(Name = "Name")] string search)
        {
            try
            {
                var lineList = await _lineService.Query(search);
                return Ok(lineList);
            }
            catch (Exception e)
            { 
                _logger.LogError($"Error on V1 Search Line API :{e.StackTrace.ToString()}");
                throw;
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CreateLineInput newLine)
        {
            if (newLine == null)
                return BadRequest(ModelState);

            var line = _mapper.Map<Line>(newLine);
            await _lineService.AddLine(line);

            return CreatedAtAction(nameof(Get), new { id = line.Id }, line);
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, UpdateLineInput updatedLine)
        {
            try
            {
                if (updatedLine == null)
                    return BadRequest(ModelState);

                await _lineService.UpdateLine(id, updatedLine);

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
            var line = await _lineService.GetLineById(id);

            if (line is null)
            {
                return NotFound($"Line with Id = {id} not found");
            }

            await _lineService.RemoveLine(id);
            return Ok($"Line with Id = {id} deleted");
        }
        
        //Get Lines of Business
        [HttpGet("Building/{buildingId}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetBuildingLines(string buildingId,DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            var lineList = await _lineService.GetLineByBuildingId(buildingId,lastUpdatedAt, isDeleted);
            return Ok(lineList);
        }
        
        //Get Lines handled by Integrator
        [HttpGet("Integrator/{integratorId}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetIntegratorLines(string integratorId,DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            var lineList = await _lineService.GetLineByIntegratorId(integratorId,lastUpdatedAt, isDeleted);
            return Ok(lineList);
        }
    }
    
   
}


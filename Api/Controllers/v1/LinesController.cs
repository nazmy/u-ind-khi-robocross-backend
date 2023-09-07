using System;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using GeoJSON.Net.Geometry;
using khi_robocross_api.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace khi_robocross_api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LinesController : ControllerBase
	{
        private readonly ILineService _lineService;
        private readonly IMapper _mapper;

        public LinesController(ILineService lineService,
            IMapper mapper)
        {
            this._lineService = lineService;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            var lineList = await _lineService.GetAllLines();
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

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CreateLineInputDto newLine)
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
        public async Task<IActionResult> Update(string id, UpdateLineInputDto updatedLine)
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
    }
}


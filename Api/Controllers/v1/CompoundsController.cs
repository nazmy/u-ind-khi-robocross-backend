using System;
using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using GeoJSON.Net.Geometry;
using khi_robocross_api.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.GeoJsonObjectModel;
using Newtonsoft.Json;

namespace khi_robocross_api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CompoundsController : ControllerBase
	{
        private readonly ICompoundService _compoundService;
        private readonly IBuildingService _buildingService;
        private readonly IMapper _mapper;

        public CompoundsController(ICompoundService compoundService,
            IBuildingService buildingService,
            IMapper mapper)
        {
            this._compoundService = compoundService;
            this._buildingService = buildingService;
            this._mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            var compoundList = await _compoundService.GetAllCompounds();
            return Ok(compoundList);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<CompoundResponse>> Get(string id)
        {
            try
            {
                var compound = await _compoundService.GetCompoundById(id);
                if (compound == null)
                {
                    return NotFound($"Compound with Id = {id} not found");
                }
                
                return Ok(compound);
            }
            catch (ArgumentException aex)
            {
                return BadRequest("Invalid Compound ID");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CreateCompoundInputDto newCompound)
        {
            
            if (newCompound == null)
                return BadRequest(ModelState);

            var compound = _mapper.Map<Compound>(newCompound);
            await _compoundService.AddCompound(compound);

            return CreatedAtAction(nameof(Get), new { id = compound.Id }, compound);
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, UpdateCompoundInputDto updatedCompound)
        {
            try
            {
                if (updatedCompound == null)
                    return BadRequest(ModelState);

                await _compoundService.UpdateCompound(id, updatedCompound);

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
            var compound = await _compoundService.GetCompoundById(id);

            if (compound is null)
            {
                return NotFound($"Compound with Id = {id} not found");
            }

            await _compoundService.RemoveCompound(id);
            return Ok($"Compound with Id = {id} deleted");
        }
        
        //Building's compound
        [HttpGet("{id}/buildings")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCompoundBuildings(string id)
        {
            var buildingList = await _buildingService.GetBuildingByCompoundId(id);
            return Ok(buildingList);
        }
    }
}


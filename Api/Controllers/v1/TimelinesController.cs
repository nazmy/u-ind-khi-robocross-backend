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
    public class TimelinesController : ControllerBase
	{
        private readonly ITimelineService _timelineService;
        private readonly IMapper _mapper;

        public TimelinesController(ITimelineService timelineService,
            IMapper mapper)
        {
            _timelineService = timelineService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            var timelineList = await _timelineService.GetAllTimelines();
            return Ok(timelineList);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TimelineResponse>> Get(string id)
        {
            try
            {
                var timeline = await _timelineService.GetTimelineById(id);
                if (timeline == null)
                {
                    return NotFound($"Timeline with Id = {id} not found");
                }

                return Ok(timeline);
            }
            catch (ArgumentException aex)
            {
                return BadRequest("Invalid Timeline ID");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CreateTimelineInput newTimeline)
        {
            if (newTimeline == null)
                return BadRequest(ModelState);
            
            var timeline = _mapper.Map<Timeline>(newTimeline);
            await _timelineService.AddTimeline(timeline);

            return CreatedAtAction(nameof(Get), new { id = timeline.Id }, timeline);
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, UpdateTimelineInput updatedTimelineInput)
        {
            try
            {
                if (updatedTimelineInput == null)
                    return BadRequest(ModelState);

                await _timelineService.UpdateTimeline(id, updatedTimelineInput);
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
            var timeline = await _timelineService.GetTimelineById(id);

            if (timeline is null)
            {
                return NotFound($"Timeline with Id = {id} not found");
            }

            await _timelineService.RemoveTimeline(id);
            return Ok($"Timeline with Id = {id} deleted");
        }
        
        //Get Timeline by Unit
        [HttpGet("Unit/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetTimelineLines(string id)
        {
            var timelineList = await _timelineService.GetTimelineByUnitId(id);
            return Ok(timelineList);
        }
    }
}


using System.Security.Claims;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using domain.Repositories;

namespace khi_robocross_api.Services
{
	public class TimelineService : ITimelineService
	{
        private readonly ITimelineRepository _timelineRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

		public TimelineService(ITimelineRepository timelineRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
            _timelineRepository = timelineRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddTimeline(Timeline inputTimeline)
        {
            if (inputTimeline == null)
                throw new ArgumentException("Timeline input is invalid");

            inputTimeline.CreateChangesTime(inputTimeline, _httpContextAccessor.HttpContext.User.Identity.Name);
            
            //validation goes here
            await _timelineRepository.CreateAsync(inputTimeline);
        }

        public async ValueTask<IEnumerable<TimelineResponse>> GetAllTimelines(DateTimeOffset? lastUpdatedAt)
        {
            var timelineTask = await _timelineRepository.GetAsync(lastUpdatedAt);
            return _mapper.Map<IEnumerable<TimelineResponse>>(timelineTask.ToList());
        }

        public async ValueTask<IEnumerable<TimelineResponse>> GetTimelineByUnitId(string unitId, DateTimeOffset? lastUpdatedAt)
        {
            var timelineTask = await _timelineRepository.GetAsyncByUnitId(unitId,lastUpdatedAt);
            return _mapper.Map<IEnumerable<TimelineResponse>>(timelineTask.ToList());
        }

        public async ValueTask<TimelineResponse> GetTimelineById(string id)
        {
            if (id == null)
                throw new ArgumentException("Timeline Id is Invalid");

            var timelineTask = await _timelineRepository.GetAsync(id);
            return _mapper.Map<TimelineResponse>(timelineTask);
        }

        public async Task RemoveTimeline(string id)
        {
            if (id == null)
                throw new ArgumentException("Timeline Id is Invalid");

            await _timelineRepository.RemoveAsync(id,_httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public async Task UpdateTimeline(string id, UpdateTimelineInput updatedTimeline)
        {
            if (id == null)
                throw new ArgumentException("Timeline Id is invalid");

            if (updatedTimeline == null)
                throw new ArgumentException("Timeline Input is invalid");
            
            var timeline = await _timelineRepository.GetAsync(id);

            if (timeline == null)
                throw new KeyNotFoundException($"Timeline with Id = {id} not found");

            _mapper.Map<UpdateTimelineInput, Timeline>(updatedTimeline,timeline);
            timeline.UpdateChangesTime(timeline, _httpContextAccessor.HttpContext.User.Identity.Name);
            
            await _timelineRepository.UpdateAsync(id, timeline);
        }
    }
}


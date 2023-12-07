using System;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public interface ITimelineService
	{
		 ValueTask<IEnumerable<TimelineResponse>> GetAllTimelines(DateTimeOffset? lastUpdatedAt,bool? isDeleted);
		 ValueTask<TimelineResponse> GetTimelineById(String id);
         ValueTask<IEnumerable<TimelineResponse>> GetTimelineByUnitId(String unitId,DateTimeOffset? lastUpdatedAt,bool? isDeleted);
         ValueTask<Timeline> AddTimeline(CreateTimelineInput createTimelineInput);
         Task UpdateTimeline(string id, UpdateTimelineInput updatedTimeline);
         Task RemoveTimeline(string id);
	}
}


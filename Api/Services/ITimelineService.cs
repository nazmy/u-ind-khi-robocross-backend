﻿using System;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public interface ITimelineService
	{
		 ValueTask<IEnumerable<TimelineResponse>> GetAllTimelines(DateTimeOffset? lastUpdatedAt);
		 ValueTask<TimelineResponse> GetTimelineById(String id);
         ValueTask<IEnumerable<TimelineResponse>> GetTimelineByUnitId(String unitId,DateTimeOffset? lastUpdatedAt);
         Task AddTimeline(Timeline inputTimeline);
         Task UpdateTimeline(string id, UpdateTimelineInput updatedTimeline);
         Task RemoveTimeline(string id);
	}
}

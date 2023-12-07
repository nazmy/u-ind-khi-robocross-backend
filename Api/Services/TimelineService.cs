using System.Diagnostics;
using System.Security.Claims;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using Domain.Helper;
using domain.Repositories;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace khi_robocross_api.Services
{
	public class TimelineService : ITimelineService
	{
        private readonly ITimelineRepository _timelineRepository;
        private readonly ITimelineDetailsRepository _timelineDetailsRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

		public TimelineService(ITimelineRepository timelineRepository,
            ITimelineDetailsRepository timelineDetailsRepository,
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
            _timelineRepository = timelineRepository;
            _timelineDetailsRepository = timelineDetailsRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async ValueTask<Timeline> AddTimeline(CreateTimelineInput createTimelineInput)
        {
            if (createTimelineInput == null)
                throw new ArgumentException("Timeline input is invalid");
            
            var timeline = _mapper.Map<Timeline>(createTimelineInput);
            timeline.CreateChangesTime(createTimelineInput, _httpContextAccessor.HttpContext.User.Identity.Name);
            
            //validation goes here
            if (createTimelineInput.TimelineDetailsIds != null && 
                createTimelineInput.TimelineDetailsIds.Length > 0)
            {
                //attach to existing timelineDetails, doesn't need to create a new timelineDetails
                 await _timelineRepository.CreateAsync(timeline);
                 return timeline;
            }
            else
            {
                List<WriteModel<TimelineDetails>> timelineDetailsList = new List<WriteModel<TimelineDetails>>();
                TimelineDetails newTimelineDetails;
                foreach (var createTimelineDetailsInput in createTimelineInput.TimelineDetails)
                {
                    newTimelineDetails = new TimelineDetails();
                    newTimelineDetails.Name = createTimelineDetailsInput.Name;
                    newTimelineDetails.Tracks = _mapper.Map<TimelineTrack[]>(createTimelineDetailsInput.Tracks);
                    newTimelineDetails.CreateChangesTime(newTimelineDetails,_httpContextAccessor.HttpContext.User.Identity.Name);
                    timelineDetailsList.Add(new InsertOneModel<TimelineDetails>(newTimelineDetails));
                }

                //validation goes here
                var resultWrites =  await _timelineDetailsRepository.CreateManyAsync(timelineDetailsList);
                var insertedTimelineDetails = resultWrites.ProcessedRequests.ToList();
                List<BulkTimelineDetailsResponse> timelineDetailsResponses = new List<BulkTimelineDetailsResponse>();

                List<string> timelineDetailsId = new List<string>();
             
                foreach (var createdTimelineDetails in insertedTimelineDetails)
                { 
                    string createdTimelineDetailString = JsonConvert.SerializeObject(createdTimelineDetails);
                    var timelineDetail = JsonConvert.DeserializeObject<WriteModelWrapper<BulkTimelineDetailsResponse>>(createdTimelineDetailString);
                    timelineDetailsResponses.Add(timelineDetail.Document);
                    
                    timelineDetailsId.Add(timelineDetail.Document.Id);
                }

                timeline.TimelineDetailsIds = timelineDetailsId.ToArray();
                await _timelineRepository.CreateAsync(timeline);
                
                return timeline;
            }
        }

        public async ValueTask<IEnumerable<TimelineResponse>> GetAllTimelines(DateTimeOffset? lastUpdatedAt,bool? isDeleted)
        {
            var timelineTask = await _timelineRepository.GetAsync(lastUpdatedAt,isDeleted);

            IEnumerable<TimelineResponse> timelineResponse = _mapper.Map<IEnumerable<TimelineResponse>>(timelineTask.ToList());
            timelineResponse= await this.updateTimelineDetails(timelineResponse);
            
            return timelineResponse;
        }

        public async ValueTask<IEnumerable<TimelineResponse>> GetTimelineByUnitId(string unitId, DateTimeOffset? lastUpdatedAt,bool? isDeleted)
        {
            var timelineTask = await _timelineRepository.GetAsyncByUnitId(unitId,lastUpdatedAt,isDeleted);
            
            IEnumerable<TimelineResponse> timelineResponse = _mapper.Map<IEnumerable<TimelineResponse>>(timelineTask.ToList());
            timelineResponse= await this.updateTimelineDetails(timelineResponse);
            
            return timelineResponse;
        }

        public async ValueTask<TimelineResponse> GetTimelineById(string id)
        {
            if (id == null)
                throw new ArgumentException("Timeline Id is Invalid");

            var timelineTask = await _timelineRepository.GetAsync(id);

            TimelineResponse timelineResponse = _mapper.Map<TimelineResponse>(timelineTask);
            timelineResponse = await this.updateTimelineDetails(timelineResponse);

            return timelineResponse;
        }

        private async ValueTask<TimelineResponse> updateTimelineDetails(TimelineResponse timelineResponse)
        {
            //list to temporarily store timelineDetailsIds that will be used for querying
            List<string> timelineDetailIds = timelineResponse.TimelineDetailsIds.ToList();
            
            //getDistinctTimelineDetailsIds
            timelineDetailIds = timelineDetailIds.Distinct().ToList();
            
            //getTimelineDetailsIds
            List<TimelineDetails> timelineDetailsList = await _timelineDetailsRepository.GetManyAsync(timelineDetailIds);
            
            List<TimelineDetailsResponse> timelineDetailsListHolder = new List<TimelineDetailsResponse>();
            //mapTimelineDetails back to timelineResponse entity
            foreach (var timelineDetailsId in timelineResponse.TimelineDetailsIds)
            {
                TimelineDetails timelineDetails = timelineDetailsList.Find(x => x.Id == timelineDetailsId);
                    
                timelineDetailsListHolder.Add(_mapper.Map<TimelineDetailsResponse>(timelineDetails)); 
            }

            timelineResponse.TimelineDetails = timelineDetailsListHolder.ToArray();
            return timelineResponse;
        }
        
        private async ValueTask<IEnumerable<TimelineResponse>> updateTimelineDetails(IEnumerable<TimelineResponse> timelineList)
        {
            //list to temporarily store timelineDetailsIds that will be used for querying
            List<string> timelineDetailIds = new List<string>(); 
            foreach (var timeline in timelineList)
            {
                foreach (var timelineDetailsId in timeline.TimelineDetailsIds)
                {
                    timelineDetailIds.Add(timelineDetailsId);
                }
            }
            
            //getDistinctTimelineDetailsIds
            timelineDetailIds = timelineDetailIds.Distinct().ToList();
            
            //getTimelineDetailsIds
            List<TimelineDetails> timelineDetailsList = await _timelineDetailsRepository.GetManyAsync(timelineDetailIds);
            
            //mapTimelineDetails back to timelineResponse entity
            foreach (var timeline in timelineList)
            {
                List<TimelineDetailsResponse> timelineDetailsListHolder = new List<TimelineDetailsResponse>();
                foreach (var timelineDetailsId in timeline.TimelineDetailsIds)
                {
                    TimelineDetails timelineDetails = timelineDetailsList.Find(x => x.Id == timelineDetailsId);
                    
                    timelineDetailsListHolder.Add(_mapper.Map<TimelineDetailsResponse>(timelineDetails));
                }
        
                timeline.TimelineDetails = timelineDetailsListHolder.ToArray();
            }

            return timelineList;
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
            
            //validation goes here
            if (updatedTimeline.TimelineDetailsIds != null && 
                updatedTimeline.TimelineDetailsIds.Length > 0)
            {
                //attach to existing timelineDetails, doesn't need to create a new timelineDetails
                 await _timelineRepository.UpdateAsync(id, timeline);
            }
            else
            {
                List<WriteModel<TimelineDetails>> timelineDetailsList = new List<WriteModel<TimelineDetails>>();
                TimelineDetails newTimelineDetails;
                foreach (var updateTimelineDetailsInput in updatedTimeline.TimelineDetails)
                {
                    newTimelineDetails = new TimelineDetails();
                    newTimelineDetails.Name = updateTimelineDetailsInput.Name;
                    newTimelineDetails.Tracks = _mapper.Map<TimelineTrack[]>(updateTimelineDetailsInput.Tracks);
                    newTimelineDetails.CreateChangesTime(newTimelineDetails,_httpContextAccessor.HttpContext.User.Identity.Name);
                    timelineDetailsList.Add(new InsertOneModel<TimelineDetails>(newTimelineDetails));
                }

                //validation goes here
                var resultWrites =  await _timelineDetailsRepository.CreateManyAsync(timelineDetailsList);
                var insertedTimelineDetails = resultWrites.ProcessedRequests.ToList();
                List<BulkTimelineDetailsResponse> timelineDetailsResponses = new List<BulkTimelineDetailsResponse>();

                List<string> timelineDetailsId = new List<string>();
             
                foreach (var createdTimelineDetails in insertedTimelineDetails)
                { 
                    string createdTimelineDetailString = JsonConvert.SerializeObject(createdTimelineDetails);
                    var timelineDetail = JsonConvert.DeserializeObject<WriteModelWrapper<BulkTimelineDetailsResponse>>(createdTimelineDetailString);
                    timelineDetailsResponses.Add(timelineDetail.Document);
                    
                    timelineDetailsId.Add(timelineDetail.Document.Id);
                }

                timeline.TimelineDetailsIds = timelineDetailsId.ToArray();
                await _timelineRepository.UpdateAsync(id, timeline);
            }
        }
    }
}


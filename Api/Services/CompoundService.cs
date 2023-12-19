using System;
using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using domain.Identity;
using domain.Repositories;
using khi_robocross_api.Helper;

namespace khi_robocross_api.Services
{
	public class CompoundService : ICompoundService
	{
        private readonly ICompoundRepository _compoundRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggedInUser _loggedInUser;

		public CompoundService(ICompoundRepository compoundRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILoggedInUser loggedInUser)
		{
            _compoundRepository = compoundRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = loggedInUser;
        }

        public async Task AddCompound(Compound inputCompound)
        {
            if (inputCompound == null)
                throw new ArgumentException("Compound input is invalid");

            inputCompound.CreateChangesTime(inputCompound, IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
            
            //validation goes here
            await _compoundRepository.CreateAsync(inputCompound);
        }

        public async ValueTask<IEnumerable<CompoundResponse>> GetAllCompounds(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var compoundTask = await _compoundRepository.GetAsync(loggedInUser,lastUpdatedAt, isDeleted); 
            return _mapper.Map<IEnumerable<CompoundResponse>>(compoundTask.ToList());
        }

        public async ValueTask<IEnumerable<CompoundResponse>> GetCompoundByClientId(string clientId, DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var compoundTask = await _compoundRepository.GetAsyncByClientId(loggedInUser, clientId, lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<CompoundResponse>>(compoundTask.ToList());
        }

        public async ValueTask<CompoundResponse> GetCompoundById(string id)
        {
            if (id == null)
                throw new ArgumentException("Compound Id is Invalid");

            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            
            var compoundTask = await _compoundRepository.GetAsync(loggedInUser, id);
            return _mapper.Map<CompoundResponse>(compoundTask);
        }
        
        public async ValueTask<IEnumerable<CompoundResponse>> Query(string search)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var compoundTask = await _compoundRepository.SearchAsync(loggedInUser, search);
            return _mapper.Map<IEnumerable<CompoundResponse>>(compoundTask.ToList());
        }

        public async Task RemoveCompound(string id)
        {
            if (id == null)
                throw new ArgumentException("Compound Id is Invalid");

            await _compoundRepository.RemoveAsync(id,IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
        }

        public async Task UpdateCompound(string id, UpdateCompoundInput updatedCompound)
        {
            if (id == null)
                throw new ArgumentException("Compound Id is invalid");

            if (updatedCompound == null)
                throw new ArgumentException("Compound Input is invalid");
            
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            
            var compound = await _compoundRepository.GetAsync(loggedInUser,id);

            if (compound == null)
                throw new KeyNotFoundException($"Compound with Id = {id} not found");

            _mapper.Map<UpdateCompoundInput,Compound>(updatedCompound,compound);
            compound.UpdateChangesTime(compound, IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
            await _compoundRepository.UpdateAsync(id, compound);
        }
    }
}


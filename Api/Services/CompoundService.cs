using System;
using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using domain.Repositories;

namespace khi_robocross_api.Services
{
	public class CompoundService : ICompoundService
	{
        private readonly ICompoundRepository _compoundRepository;
        private readonly IMapper _mapper;

		public CompoundService(ICompoundRepository compoundRepository, IMapper mapper)
		{
            _compoundRepository = compoundRepository;
            _mapper = mapper;
        }

        public async Task AddCompound(Compound inputCompound)
        {
            if (inputCompound == null)
                throw new ArgumentException("Compound input is invalid");

            inputCompound.CreateChangesTime(inputCompound);
            
            //validation goes here
            await _compoundRepository.CreateAsync(inputCompound);
        }

        public async ValueTask<IEnumerable<CompoundResponse>> GetAllCompounds()
        {
            var compoundTask = await _compoundRepository.GetAsync();
            
            if (compoundTask != null)
                return _mapper.Map<IEnumerable<CompoundResponse>>(compoundTask.ToList());

            return null;
        }

        public async ValueTask<IEnumerable<CompoundResponse>> GetCompoundByClientId(string clientId)
        {
            var compoundTask = await _compoundRepository.GetAsyncByClientId(clientId);
            if (compoundTask != null)
                return _mapper.Map<IEnumerable<CompoundResponse>>(compoundTask.ToList());
            
            return null;
        }

        public async ValueTask<CompoundResponse> GetCompoundById(string id)
        {
            if (id == null)
                throw new ArgumentException("Compound Id is Invalid");

            var compoundTask = await _compoundRepository.GetAsync(id);
            
            if (compoundTask != null)
                return _mapper.Map<CompoundResponse>(compoundTask);

            return null;
        }

        public async Task RemoveCompound(string id)
        {
            if (id == null)
                throw new ArgumentException("Compound Id is Invalid");

            await _compoundRepository.RemoveAsync(id);
        }

        public async Task UpdateCompound(string id, UpdateCompoundInput updatedCompound)
        {
            if (id == null)
                throw new ArgumentException("Compound Id is invalid");

            if (updatedCompound == null)
                throw new ArgumentException("Compound Input is invalid");
            
            var compound = await _compoundRepository.GetAsync(id);

            if (compound == null)
                throw new KeyNotFoundException($"Compound with Id = {id} not found");

            compound = _mapper.Map<Compound>(updatedCompound);
            compound.UpdateChangesTime(compound);
            
            await _compoundRepository.UpdateAsync(id, compound);
        }
    }
}


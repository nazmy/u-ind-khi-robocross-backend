using System;
using AutoMapper;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public class ClientService : IClientService
	{
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

		public ClientService(IClientRepository clientRepository, IMapper mapper)
		{
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task AddClient(Client inputClient)
        {
            if (inputClient == null)
                throw new ArgumentException("Client input is invalid");

            inputClient.CreateChangesTime(inputClient);
            
            //validation goes here
            await _clientRepository.CreateAsync(inputClient);
        }

        public async ValueTask<IEnumerable<ClientOutputDto>> GetAllClients()
        {
            var clientTask = await _clientRepository.GetAsync();
            if (clientTask != null)
                return _mapper.Map<IEnumerable<ClientOutputDto>>(clientTask.ToList());

            return null;
        }

        public async ValueTask<ClientOutputDto> GetClientById(string id)
        {
            if (id == null)
                throw new ArgumentException("Client Id is Invalid");

            var clientTask = await _clientRepository.GetAsync(id);
            if (clientTask != null)
                return _mapper.Map<ClientOutputDto>(clientTask);

            return null;
        }

        public async Task RemoveClient(string id)
        {
            if (id == null)
                throw new ArgumentException("Client Id is Invalid");

            await _clientRepository.RemoveAsync(id);
        }

        public async Task UpdateClient(string id, UpdateClientInputDto updatedClient)
        {
            if (id == null)
                throw new ArgumentException("ClientId is invalid");

            if (updatedClient == null)
                throw new ArgumentException("Client Input is invalid");
            
            var client = await _clientRepository.GetAsync(id);

            if (client == null)
                throw new KeyNotFoundException($"Client with Id = {id} not found");

            client = _mapper.Map<Client>(updatedClient);

            client.UpdateChangesTime(client);
            
            await _clientRepository.UpdateAsync(id, client);
        }
    }
}


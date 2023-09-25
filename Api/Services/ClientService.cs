using System;
using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using MongoDB.Driver;

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

        public async ValueTask<IEnumerable<ClientResponse>> GetAllClients()
        {
            var clientTask = await _clientRepository.GetAsync();
            if (clientTask != null)
                return _mapper.Map<IEnumerable<ClientResponse>>(clientTask.ToList());

            return null;
        }

        public async ValueTask<ClientResponse> GetClientById(string id)
        {
            if (id == null)
                throw new ArgumentException("Client Id is Invalid");

            var clientTask = await _clientRepository.GetAsync(id);
            if (clientTask != null)
                return _mapper.Map<ClientResponse>(clientTask);

            return null;
        }

        public async ValueTask<IEnumerable<ClientResponse>> Query(string search)
        {
            var clientTask = await _clientRepository.SearchAsync(search);
            if (clientTask != null)
                return _mapper.Map<IEnumerable<ClientResponse>>(clientTask.ToList());

            return null;
        }

        public async Task RemoveClient(string id)
        {
            if (id == null)
                throw new ArgumentException("Client Id is Invalid");

            await _clientRepository.RemoveAsync(id);
        }

        public async Task UpdateClient(string id, UpdateClientInput updatedClient)
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


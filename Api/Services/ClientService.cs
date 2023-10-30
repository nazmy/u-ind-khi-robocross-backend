using System;
using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using domain.Repositories;
using MongoDB.Driver;

namespace khi_robocross_api.Services
{
	public class ClientService : IClientService
	{
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

		public ClientService(IClientRepository clientRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
		{
            _clientRepository = clientRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddClient(Client inputClient)
        {
            if (inputClient == null)
                throw new ArgumentException("Client input is invalid");

            inputClient.CreateChangesTime(inputClient,  _httpContextAccessor.HttpContext.User.Identity.Name);
            
            //validation goes here
            await _clientRepository.CreateAsync(inputClient);
        }

        public async ValueTask<IEnumerable<ClientResponse>> GetAllClients(DateTimeOffset? lastUpdatedAt)
        {
            var clientTask = await _clientRepository.GetAsync(lastUpdatedAt);
            return _mapper.Map<IEnumerable<ClientResponse>>(clientTask.ToList());
        }

        public async ValueTask<ClientResponse> GetClientById(string id)
        {
            if (id == null)
                throw new ArgumentException("Client Id is Invalid");

            var clientTask = await _clientRepository.GetAsync(id);
            return _mapper.Map<ClientResponse>(clientTask);
        }

        public async ValueTask<IEnumerable<ClientResponse>> Query(string search)
        {
            var clientTask = await _clientRepository.SearchAsync(search);
            return _mapper.Map<IEnumerable<ClientResponse>>(clientTask.ToList());
        }

        public async Task RemoveClient(string id)
        {
            if (id == null)
                throw new ArgumentException("Client Id is Invalid");

            await _clientRepository.RemoveAsync(id,_httpContextAccessor.HttpContext.User.Identity.Name);
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
            
            _mapper.Map<UpdateClientInput, Client>(updatedClient,client);
            client.UpdateChangesTime(client, _httpContextAccessor.HttpContext.User.Identity.Name);
            await _clientRepository.UpdateAsync(id, client);
        }
    }
}


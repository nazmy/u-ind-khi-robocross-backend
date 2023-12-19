using System;
using AutoMapper;
using Domain.Dto;
using Domain.Entities;
using domain.Identity;
using domain.Repositories;
using khi_robocross_api.Helper;
using MongoDB.Driver;

namespace khi_robocross_api.Services
{
	public class ClientService : IClientService
	{
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoggedInUser _loggedInUser;

		public ClientService(IClientRepository clientRepository, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor,
            ILoggedInUser loggedInUser)
		{
            _clientRepository = clientRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _loggedInUser = loggedInUser;
        }

        public async Task AddClient(Client inputClient)
        {
            if (inputClient == null)
                throw new ArgumentException("Client input is invalid");

            inputClient.CreateChangesTime(inputClient,  IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
            
            //validation goes here
            await _clientRepository.CreateAsync(inputClient);
        }

        public async ValueTask<IEnumerable<ClientResponse>> GetAllClients(DateTimeOffset? lastUpdatedAt, bool? isDeleted)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var clientTask = await _clientRepository.GetAsync(loggedInUser,lastUpdatedAt, isDeleted);
            return _mapper.Map<IEnumerable<ClientResponse>>(clientTask.ToList());
        }

        public async ValueTask<ClientResponse> GetClientById(string id)
        {
            if (id == null)
                throw new ArgumentException("Client Id is Invalid");
            
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();

            var clientTask = await _clientRepository.GetAsync(loggedInUser,id);
            return _mapper.Map<ClientResponse>(clientTask);
        }

        public async ValueTask<IEnumerable<ClientResponse>> Query(string search)
        {
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var clientTask = await _clientRepository.SearchAsync(loggedInUser,search);
            return _mapper.Map<IEnumerable<ClientResponse>>(clientTask.ToList());
        }

        public async Task RemoveClient(string id)
        {
            if (id == null)
                throw new ArgumentException("Client Id is Invalid");

            await _clientRepository.RemoveAsync(id,IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
        }

        public async Task UpdateClient(string id, UpdateClientInput updatedClient)
        {
            if (id == null)
                throw new ArgumentException("ClientId is invalid");

            if (updatedClient == null)
                throw new ArgumentException("Client Input is invalid");
            
            LoggedInUser loggedInUser = await _loggedInUser.GetLoggedInUser();
            var client = await _clientRepository.GetAsync(loggedInUser,id);

            if (client == null)
                throw new KeyNotFoundException($"Client with Id = {id} not found");
            
            _mapper.Map<UpdateClientInput, Client>(updatedClient,client);
            client.UpdateChangesTime(client, IdentitiesHelper.GetUserIdFromClaimPrincipal(_httpContextAccessor.HttpContext.User));
            await _clientRepository.UpdateAsync(id, client);
        }
    }
}


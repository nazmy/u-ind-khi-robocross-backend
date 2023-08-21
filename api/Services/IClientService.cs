using System;
using Domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public interface IClientService
	{
		 ValueTask<IEnumerable<ClientOutputDto>> GetAllClients();
		 ValueTask<ClientOutputDto> GetClientById(String id);
		 Task AddClient(Client client);
         Task UpdateClient(string id, UpdateClientInputDto updatedClient);
         Task RemoveClient(string id);
	}
}


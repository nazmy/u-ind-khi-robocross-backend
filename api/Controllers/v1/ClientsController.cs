using System;
using Microsoft.AspNetCore.Mvc;
using khi_robocross_api.Services;
using Domain.Entities;
using Domain.Dto;
using AutoMapper;

namespace khi_robocross_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
	{
		private readonly IClientRepository _clientRepository;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;

		public ClientsController(IClientRepository clientRepository,
            IClientService clientService,
            IMapper mapper)
		{
			this._clientRepository = clientRepository;
            this._clientService = clientService;
            this._mapper = mapper;
		}

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            var clientList = await _clientService.GetAllClients();
            return Ok(clientList);
        }
           

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientOutputDto>> Get(string id)
        {
            try
            {
                var client = await _clientService.GetClientById(id);
                if (client == null)
                {
                    return NotFound($"Client with Id = {id} not found");
                }

                return Ok(client);
            }
            catch (ArgumentException aex)
            {
                return BadRequest("Invalid Client ID");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CreateClientInputDto newClient)
		{
            if (newClient == null)
                return BadRequest(ModelState);

            var client = _mapper.Map<Client>(newClient);
            await _clientService.AddClient(client);
			return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
		}

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, UpdateClientInputDto updatedClient)
        {
            try
            {

                if (updatedClient == null)
                    return BadRequest(ModelState);

                await _clientService.UpdateClient(id, updatedClient);

                return NoContent();
            }
            catch(ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (KeyNotFoundException kex)
            {
                return NotFound(kex.Message);
            }
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            var client = await _clientRepository.GetAsync(id);

            if (client is null)
            {
                return NotFound($"Client with Id = {id} not found");
            }

            await _clientRepository.RemoveAsync(id);
            return Ok($"Client with Id = {id} deleted");
        }
	}
}


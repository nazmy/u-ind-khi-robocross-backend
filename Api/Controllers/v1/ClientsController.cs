using System;
using System.Runtime.CompilerServices;
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
		private readonly IClientService _clientService;
        private readonly ICompoundService _compoundService;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientsController> _logger;

		public ClientsController(IClientService clientService,
            IMapper mapper)
		{
			this._clientService = clientService;
            this._mapper = mapper;
            this._logger = new LoggerFactory().CreateLogger<ClientsController>();
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ClientResponse>))]
        public async Task<IActionResult> Get()
        {
            try
            {
                var clientList = await _clientService.GetAllClients();
                return Ok(clientList);
            }
            catch (Exception e)
            { 
                _logger.LogError($"Error on V1 GetClient API :{e.StackTrace.ToString()}");
                throw;
            }
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(200, Type = typeof(ClientResponse))]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientResponse>> Get(string id)
        {
            try
            {
                var client = await _clientService.GetClientById(id);
                if (client == null)
                {
                    _logger.LogDebug($"V1 GetClient API with Id = {id} not found");
                    return NotFound($"Client with Id = {id} not found");
                }

                return Ok(client);
            }
            catch (ArgumentException aex)
            {
                _logger.LogWarning("V1 GetClient API BadRequest Parameter Client ID");
                return BadRequest("Invalid Client ID");
            }
            catch (Exception e)
            {
                _logger.LogError($"Error on V1 GetClient API by Id {id} : {e.StackTrace.ToString()}");
                throw;
            }
        }

        [HttpGet("Clients/Search")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ClientResponse>))]
        public async Task<ActionResult<ClientResponse>> Search([FromQuery(Name = "Name")] string search)
        {
            try
            {
                var clientList = await _clientService.Query(search);
                return Ok(clientList);
            }
            catch (Exception e)
            { 
                _logger.LogError($"Error on V1 GetClient API :{e.StackTrace.ToString()}");
                throw;
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CreateClientInputDto newClient)
		{
            if (newClient == null)
            {
                _logger.LogWarning("V1 CreateClient API BadRequest Client Data");
                return BadRequest(ModelState);
            }
            
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
            var client = await _clientService.GetClientById(id);

            if (client is null)
            {
                return NotFound($"Client with Id = {id} not found");
            }

            await _clientService.RemoveClient(id);
            return Ok($"Client with Id = {id} deleted");
        }

        //Client's compound

        [HttpGet("{id:int}/compounds")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetClientCompounds(string id)
        {
            var compoundList = await _compoundService.GetCompoundByClientId(id);
            return Ok(compoundList);
        }
    }
}


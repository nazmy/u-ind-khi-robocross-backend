using System;
using Microsoft.AspNetCore.Mvc;
using khi_robocross_api.Services;
using Domain.Models;

namespace khi_robocross_api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
	{
		private readonly IClientService clientService;
		public ClientsController(IClientService clientService)
		{
			this.clientService = clientService;
		}

        [HttpGet]
        public async Task<List<Client>> Get() =>
            await clientService.GetAsync();


        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> Get(string id)
        {
            var client = await clientService.GetAsync(id);

            if (client == null)
            {
                return NotFound($"Client with Id = {id} not found");
            }

            return client;
        }

        [HttpPost]
		public async Task<IActionResult> Post([FromBody] Client client)
		{
			await clientService.CreateAsync(client);
			return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
		}

	}
}


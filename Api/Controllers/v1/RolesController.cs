using AspNetCore.Identity.MongoDbCore.Models;
using AutoMapper;
using domain.Dto;
using domain.Repositories;
using khi_robocross_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace khi_robocross_api.Controllers.v1;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ILogger<RolesController> _logger;
    private readonly IMapper _mapper;

    public RolesController(RoleManager<AppRole> roleManager, 
        IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
        _logger = new LoggerFactory().CreateLogger<RolesController>();
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<RoleResponse>))]
    public async Task<IActionResult> Get()
    {
        try
        {
            List<AppRole> roleList = _roleManager.Roles.ToList();
            var roleListResponse = _mapper.Map<List<RoleResponse>>(roleList);
            return Ok(roleListResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error on V1 GetRoles API :{ex.StackTrace.ToString()}");
            throw;
        }
    }
    
    // [HttpPost]
    // [ProducesResponseType(201)]
    // [ProducesResponseType(400)]
    // [ProducesResponseType(500)]
    // public async Task<IActionResult> Post([FromBody] CreateRoleInput createRoleInput)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }
    //
    //     AppRole appRole = new AppRole()
    //     {
    //         Name = createRoleInput.Name,
    //         ConcurrencyStamp = DateTimeOffset.UtcNow.ToString(),
    //         Description = createRoleInput.Description
    //     };
    //
    //     IdentityResult result = await _roleManager.CreateAsync(appRole);
    //     
    //     if (result.Succeeded)
    //     {
    //         return CreatedAtAction(nameof(Get), new { id = appRole.Id }, appRole);    
    //     }
    //     else
    //     {
    //         foreach (IdentityError error in result.Errors)
    //             ModelState.AddModelError(error.Code, error.Description); 
    //             
    //         return BadRequest(ModelState);
    //     }
    // }
}
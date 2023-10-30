using System;
using System.Security.Claims;
using AutoMapper;
using domain.Dto;
using Domain.Dto;
using Domain.Entities;
using domain.Repositories;
using domain.Repositories.Extensions;
using GeoJSON.Net.Geometry;
using khi_robocross_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;

namespace khi_robocross_api.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
	{
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;

        public UserController(UserManager<AppUser> userManager,
            IMapper mapper,
            IPasswordHasher<AppUser> passwordHasher)
        {
            _userManager = userManager;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(DateTimeOffset? lastUpdatedAt)
        {
            List<AppUser> appUserList;
            if (lastUpdatedAt != null)
            {
                appUserList = _userManager.Users
                    .Where(user => user.LastUpdatedAt >= lastUpdatedAt)
                    .ToList();
            }
            else
            {
               appUserList = _userManager.Users
                   .OrderByDescending(user =>user.LastUpdatedAt) 
                   .ToList();
            }
            
            var userListResponse = _mapper.Map<List<UserResponse>>(appUserList);
            return Ok(userListResponse);
        }

        [HttpGet("{id:length(24)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UserResponse>> Get(string id)
        {
            try
            {
                AppUser user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"User with Id = {id} not found");
                }
        
                return Ok(_mapper.Map<UserResponse>(user));
            }
            catch (ArgumentException aex)
            {
                return BadRequest("Invalid User ID");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] CreateUserInput newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!newUser.Password.Equals(newUser.ConfirmedPassword))
            {
                return BadRequest("Password doesn't match with Confirmed Password");
            }

            List<ObjectId> roleId = new List<ObjectId>();
            roleId.Add(MongoDB.Bson.ObjectId.Parse(newUser.RoleId));

            AppUser appUser = new AppUser()
            {
                Email = newUser.EmailAddress,
                SecurityStamp = DateTimeOffset.UtcNow.ToString(),
                ConcurrencyStamp = DateTimeOffset.UtcNow.ToString(),
                Roles = roleId,
                Fullname = newUser.Fullname,
                ClientId = String.IsNullOrEmpty(newUser.ClientId) ? null : newUser.ClientId
            };
            
            
            IdentityResult result = await _userManager.CreateAsync(appUser, newUser.Password);
            if (result.Succeeded)
            {
                return CreatedAtAction(nameof(Get), new { id = appUser.Id }, appUser);    
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description); 
                
                return BadRequest(ModelState);
            }
            
        }

        [HttpPut("{id:length(24)}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, UpdateUserInput updatedUser)
        {
            try
            {
                AppUser appUser = await _userManager.FindByIdAsync(id);
            
                if (appUser is null)
                {
                    return NotFound($"User with Id = {id} not found");
                }

                if (string.IsNullOrEmpty(updatedUser.EmailAddress))
                {
                    ModelState.AddModelError("","Email cannot be empty");
                }
                
                if (ModelState.IsValid)
                {
                    appUser.Email = updatedUser.EmailAddress;
                    appUser.UserName  = updatedUser.EmailAddress;
                    appUser.Fullname = updatedUser.Fullname;
                    appUser.LastUpdatedAt = DateTimeOffset.UtcNow;
                    if (!String.IsNullOrEmpty(updatedUser.RoleId))
                    {
                        List<ObjectId> roleId = new List<ObjectId>();
                        roleId.Add(MongoDB.Bson.ObjectId.Parse(updatedUser.RoleId));

                        appUser.Roles = roleId;
                    }
                        
                    
                    IdentityResult updateUserResult = await _userManager.UpdateAsync(appUser);

                    if (updateUserResult.Succeeded)
                    {
                        return NoContent();
                    }
                    else
                    {
                        foreach (IdentityError error in updateUserResult.Errors)
                            ModelState.AddModelError(error.Code, error.Description); 
                    }
                }
               
                return BadRequest(ModelState);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
            catch (KeyNotFoundException kex)
            {
                return NotFound(kex.Message);
            }
        }
        
        [HttpPost("{id:length(24)}/ChangePassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ChangePassword(string id, UserChangePasswordInput userChangePassword)
        {
            if (!userChangePassword.Password.Equals(userChangePassword.ConfirmedPassword))
            {
                return BadRequest("Password doesn't match with Confirmed Password");
            }
            
            AppUser appUser = await _userManager.FindByIdAsync(id);
            
            if (appUser is null)
            {
                return NotFound($"User with Id = {id} not found");
            }
            
            IdentityResult result =
                await _userManager.ChangePasswordAsync(appUser, userChangePassword.CurrentPassword, userChangePassword.Password);
            
            if (result.Succeeded)
            {
                return Ok($"User password of UserId = {id} has been updated");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                
                return BadRequest(ModelState);
            }
        }
        
        [HttpPost("{id:length(24)}/Enabled")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Enabled(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            
            if (appUser is null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            IdentityResult result =
                await _userManager.SetLockoutEndDateAsync(appUser, null);
            
            if (result.Succeeded)
            {
                return Ok($"User with Id = {id} is unlocked");
            }
            else
            {
                return Problem($"Error in unlocking User with Id = {id}");
            }
        }
        
        [HttpPost("{id:length(24)}/Disabled")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Disabled(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            
            if (appUser is null)
            {
                return NotFound($"User with Id = {id} not found");
            }

            IdentityResult result =
                await _userManager.SetLockoutEndDateAsync(appUser, DateTimeOffset.Now.AddYears(1));
            
            if (result.Succeeded)
            {
                return Ok($"User with Id = {id} is locked");
            }
            else
            {
                return Problem($"Error in locked User with Id = {id}");
            }
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            
            if (appUser is null)
            {
                return NotFound($"User with Id = {id} not found");
            }
            
            IdentityResult result = await _userManager.DeleteAsync(appUser);
            if (result.Succeeded)
            {
                return Ok($"User with Id = {id} deleted");
            }
            else
            {
                return Problem($"Error in Deleting User with Id = {id}");
            }
        }
        
        //Get User By Client Id
        [HttpGet("Clients/{clientId}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetUserByClientId(string clientId, DateTimeOffset? lastUpdatedAt)
        {
            List<AppUser> appUserList = AppUserManagerExtensions.FindByClientIdAsync(_userManager, clientId, lastUpdatedAt);
            List<UserResponse> userListResponse = _mapper.Map<List<UserResponse>>(appUserList);
            return Ok(userListResponse);
        }
    }
}


using domain.Dto;
using Domain.Entities;

namespace khi_robocross_api.Services
{
	public interface IUserService
	{
		 ValueTask<IEnumerable<UserResponse>> GetAllUsers();
		 ValueTask<UserResponse> GetUserById(String id);
         ValueTask<IEnumerable<UserResponse>> GetUserByClientId(String clientId);
         ValueTask<IEnumerable<UserResponse>> GetUserByRoleId(String roleId);
         Task AddUser(User inputUser);
         Task UpdateUser(string id, UpdateUserInput updatedUser);
         Task RemoveUser(string id);
	}
}


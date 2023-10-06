using domain.Dto;

namespace khi_robocross_api.Services;

public interface IRoleService
{
    ValueTask<IEnumerable<RoleResponse>> GetAllRoles();
}
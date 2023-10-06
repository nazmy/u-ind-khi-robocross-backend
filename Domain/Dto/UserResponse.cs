namespace domain.Dto;

public class UserResponse : BaseDto
{
    public string Id { get; set; }
    
    public string Username { get; set; }

    public string Email { get; set; }
    
    public string Fullname { get; set; }
    
    public string RoleId { get; set; }
    
    public string? ClientId { get; set; }
}
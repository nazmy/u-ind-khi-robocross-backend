namespace domain.Dto;

public class UserResponse : BaseDto
{
    public string Id { get; set; }
    
    public string EmailAddress { get; set; }
    
    public string Fullname { get; set; }
    public string RoleId { get; set; }

    public DateTimeOffset LockoutEnd { get; set; }
    
    public string? ClientId { get; set; }
}
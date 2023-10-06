namespace domain.Dto;

public class UpdateUserInput
{
    public string Fullname { get; set; }

    public string EmailAddress { get; set; }
    
    public string Password { get; set; }
    
    public string ConfirmedPassword { get; set; }
    
    public string RoleId { get; set; }
    
    public string? ClientId { get; set; }
}
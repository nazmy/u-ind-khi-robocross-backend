namespace domain.Dto;

public class UserChangePasswordInput
{
    public string CurrentPassword { get; set; }
    
    public string Password { get; set; }
    
    public string ConfirmedPassword { get; set; }
    
}
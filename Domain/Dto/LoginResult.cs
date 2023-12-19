using System.Text.Json.Serialization;

namespace domain.Dto;

public class LoginResult
{
    
    [JsonPropertyName("userId")]
    public string UserId { get; set; }
    
    [JsonPropertyName("username")]
    public string UserName { get; set; }

    [JsonPropertyName("role")]
    public string Role { get; set; }
    
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }

    [JsonPropertyName("originalUserName")]
    public string OriginalUserName { get; set; }

    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}
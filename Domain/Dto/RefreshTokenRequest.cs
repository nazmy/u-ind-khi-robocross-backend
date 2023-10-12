using System.Text.Json.Serialization;

namespace domain.Dto;

public class RefreshTokenRequest
{
    [JsonPropertyName("refreshToken")] 
    public string RefreshToken { get; set; }
}
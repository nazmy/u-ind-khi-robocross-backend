using System.Text.Json.Serialization;

namespace domain.Dto;

public class ImpersonateRequest
{ 
        [JsonPropertyName("username")] 
        public string UserName { get; set; }
}
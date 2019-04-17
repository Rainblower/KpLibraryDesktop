using Newtonsoft.Json;

namespace KpLibrary.Model
{
    public class User
    {
        [JsonProperty("group")] public string Group { get; set; }

        [JsonProperty("email")] public string Email { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("role")] public string Role { get; set; }

        [JsonProperty("uid")] public string Uid { get; set; }

        [JsonProperty("status")] public string Status { get; set; }
    }
}
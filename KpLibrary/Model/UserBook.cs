using Newtonsoft.Json;

namespace KpLibrary.Model
{
    public class UserBook
    {
        [JsonProperty("bookUid")] public string BookUid { get; set; }

        [JsonProperty("userUid")] public string UserUid { get; set; }
    }
}
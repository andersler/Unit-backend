using Newtonsoft.Json;

namespace REST_API_UNIT.Models
{
    public class Unit
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = string.Empty;
        public required string Name { get; set; }
        public required bool IsActive { get; set; }
        public required string Type { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}

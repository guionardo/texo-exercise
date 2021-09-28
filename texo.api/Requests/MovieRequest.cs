using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace texo.api.Requests
{
    public class MovieRequest
    {
        [JsonPropertyName("id")] public int? Id { get; set; }
        [JsonPropertyName("release")] public int Release { get; set; }

        [JsonPropertyName("title")] public string Title { get; set; }

        [JsonPropertyName("studios")] public IEnumerable<string> Studios { get; set; }

        [JsonPropertyName("producers")] public IEnumerable<string> Producers { get; set; }

        [JsonPropertyName("winner")] public bool Winner { get; set; }
    }
}
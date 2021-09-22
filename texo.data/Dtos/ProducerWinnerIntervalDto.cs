using System.Text.Json.Serialization;

namespace texo.data.Dtos
{
    public class ProducerWinnerIntervalDto
    {
        [JsonPropertyName("producer")] public string Producer { get; set; }

        [JsonPropertyName("interval")]
        public int Interval => FollowingWin - PreviousWin;

        [JsonPropertyName("previousWin")] public int PreviousWin { get; set; }

        [JsonPropertyName("folowingWin")] public int FollowingWin { get; set; }
    }
}
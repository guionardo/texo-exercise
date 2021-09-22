using System.Collections.Generic;
using System.Text.Json.Serialization;
using texo.data.Dtos;

namespace texo.api.Responses
{
    public class WinningProducers
    {
        [JsonPropertyName("min")]
        public IEnumerable<ProducerWinnerIntervalDto> Min { get; set; }
        
        [JsonPropertyName("max")]
        public IEnumerable<ProducerWinnerIntervalDto> Max { get; set; }
    }
}
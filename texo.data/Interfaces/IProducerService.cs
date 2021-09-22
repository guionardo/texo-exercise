using System.Collections.Generic;
using System.Threading.Tasks;
using texo.data.Dtos;
using texo.domain.Entities;

namespace texo.data.Interfaces
{
    public interface IProducerService
    {
        Task<IEnumerable<Movie>> GetWinnersFromProducer(int producerId);

        Task<IEnumerable<Movie>> GetWinners();

        Task<IEnumerable<ProducerWinnerIntervalDto>> GetWiningsIntervalsFromProducer(int producerId);
        Task<IEnumerable<ProducerWinnerIntervalDto>> GetWiningsIntervals();

        Task<(IEnumerable<ProducerWinnerIntervalDto>, IEnumerable<ProducerWinnerIntervalDto>)>
            GetFirstAndLastWinProducers();
    }
}
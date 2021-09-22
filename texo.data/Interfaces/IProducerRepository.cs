using System.Collections.Generic;
using System.Threading.Tasks;
using texo.commons.Interfaces;
using texo.data.Dtos;
using texo.data.Entities;

namespace texo.data.Interfaces
{
    public interface IProducerRepository : IRepository<ProducerModel>
    {
        Task<IEnumerable<int>> GetWinnerProducers();
        Task<IEnumerable<ProducerWinnerIntervalDto>> GetWinnerIntervalsFromProducer(int producerId);
    }
}
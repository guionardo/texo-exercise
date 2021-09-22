using Microsoft.Extensions.Logging;
using texo.data.Abstractions;
using texo.data.Interfaces;
using texo.domain.Entities;

namespace texo.data.Repositories
{
    public class ProducerRepository : AbstractRepository<Producer>, IProducerRepository
    {
        public ProducerRepository(ILogger<ProducerRepository> logger, IDatabaseBootstrap databaseBootstrap) : base(
            logger,
            databaseBootstrap, nameof(Producer.Name), (Producer producer) => producer.Name)
        {
        }
    }
}
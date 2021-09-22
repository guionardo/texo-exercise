using Microsoft.Extensions.Logging;
using texo.data.Abstractions;
using texo.data.Entities;
using texo.data.Extensions;
using texo.data.Interfaces;
using texo.domain.Entities;
using Movie = texo.data.Entities.Movie;

namespace texo.data.Repositories
{
    public class AssociationMovieProducersRepository : AssociationAbstractRepository<Movie, Producer>,
        IAssociationMovieProducersRepository
    {
        public AssociationMovieProducersRepository(
            ILogger<AssociationMovieProducersRepository> logger,
            IDatabaseBootstrap databaseBootstrap) : base(
            logger, databaseBootstrap,
            nameof(MovieProducer.IdMovie).GetAsSnakeCase(),
            nameof(MovieProducer.IdProducer).GetAsSnakeCase())
        {
        }
    }
}
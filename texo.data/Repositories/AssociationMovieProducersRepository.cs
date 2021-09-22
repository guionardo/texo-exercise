using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using texo.data.Abstractions;
using texo.data.Entities;
using texo.data.Extensions;
using texo.data.Interfaces;

namespace texo.data.Repositories
{
    public class AssociationMovieProducersRepository :
        AssociationAbstractRepository<MovieModel, ProducerModel, MovieProducerModel>,
        IAssociationMovieProducersRepository
    {
        public AssociationMovieProducersRepository(
            ILogger<AssociationMovieProducersRepository> logger,
            IDatabaseBootstrap databaseBootstrap) : base(
            logger, databaseBootstrap,
            nameof(MovieProducerModel.IdMovie).GetAsSnakeCase(),
            nameof(MovieProducerModel.IdProducer).GetAsSnakeCase())
        {
        }

        public async Task<IEnumerable<ProducerModel>> GetProducersFromMovie(int movieId)
        {
            await using var db = DatabaseBootstrap.GetConnection();
            var producers = await db.QueryAsync<ProducerModel>(
                "SELECT p.* FROM movie_producers mp LEFT JOIN producers p on p.id==mp.id_producer WHERE mp.id_movie = @Id",
                new { Id = movieId });
            return producers;
        }
    }
}
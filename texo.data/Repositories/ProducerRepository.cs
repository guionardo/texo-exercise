using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using texo.data.Abstractions;
using texo.data.Dtos;
using texo.data.Entities;
using texo.data.Interfaces;

namespace texo.data.Repositories
{
    public class ProducerRepository : AbstractRepository<ProducerModel>, IProducerRepository
    {
        public ProducerRepository(ILogger<ProducerRepository> logger, IDatabaseBootstrap databaseBootstrap) : base(
            logger,
            databaseBootstrap, nameof(ProducerModel.Name), producer => producer.Name)
        {
        }

        public async Task<IEnumerable<int>> GetWinnerProducers()
        {
            await using var db = DatabaseBootstrap.GetConnection();
            var producerIds = await db.QueryAsync<int>(@"SELECT mp.id_producer,count(mp.id_producer) as c
            FROM movie_producers mp
                LEFT JOIN movies m on m.id=mp.id_movie
            WHERE m.winner = 1
            GROUP BY mp.id_producer
                HAVING c>1;");
            return producerIds;
        }

        public async Task<IEnumerable<ProducerWinnerIntervalDto>> GetWinnerIntervalsFromProducer(int producerId)
        {
            var producer = await Get(producerId);
            await using var db = DatabaseBootstrap.GetConnection();

            var movies = (await db.QueryAsync<MovieModel>(@"SELECT m.* 
FROM movies m 
LEFT JOIN movie_producers p ON p.id_movie=m.id
WHERE m.winner = 1 and p.id_producer = @Id; ", new { Id = producerId })).ToArray();
            MovieModel last = null;
            var result = new List<ProducerWinnerIntervalDto>();
            foreach (var movie in movies)
            {
                if (last != null)
                {
                    result.Add(new ProducerWinnerIntervalDto
                    {
                        Producer = producer.Name,
                        PreviousWin = last.Release,
                        FollowingWin = movie.Release
                    });
                }

                last = movie;
            }

            return result;
        }
    }
}
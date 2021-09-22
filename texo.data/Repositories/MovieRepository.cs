using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using texo.data.Abstractions;
using texo.data.Interfaces;
using texo.data.Entities;

namespace texo.data.Repositories
{
    public class MovieRepository : AbstractRepository<MovieModel>, IMovieRepository
    {
        public MovieRepository(ILogger<MovieRepository> logger, IDatabaseBootstrap databaseBootstrap) : base(logger,
            databaseBootstrap, nameof(MovieModel.Title), movie => movie.Title)
        {
        }



        public async Task<IEnumerable<MovieModel>> GetWinnerMoviesFromProducer(int producerId)
        {
            await using var db = DatabaseBootstrap.GetConnection();

            var movies =
                await db.QueryAsync<MovieModel>(
                    "SELECT m.* FROM movies m LEFT JOIN movie_producers p on p.id_movie=m.id WHERE m.winner = 1 AND p.id_producer=@Id",
                    new { Id = producerId });

            return movies;
        }

        public async Task<IEnumerable<MovieModel>> GetWinnerMovies()
        {
            await using var db = DatabaseBootstrap.GetConnection();
            var movies =
                await db.QueryAsync<MovieModel>("SELECT * FROM movies WHERE winner = 1");
            return movies;
        }
    }
}
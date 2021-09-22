using Microsoft.Extensions.Logging;
using texo.data.Abstractions;
using texo.data.Interfaces;
using texo.data.Entities;

namespace texo.data.Repositories
{
    public class MovieRepository : AbstractRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ILogger<MovieRepository> logger, IDatabaseBootstrap databaseBootstrap) : base(logger,
            databaseBootstrap, nameof(Movie.Title), (Movie movie) => movie.Title)
        {
        }
    }
}
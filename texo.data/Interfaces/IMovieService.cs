using System.Collections.Generic;
using System.Threading.Tasks;
using texo.data.Entities;
using texo.data.Enums;
using texo.domain.Entities;

namespace texo.data.Interfaces
{
    public interface IMovieService
    {
        Task<Movie> GetMovieById(int id);
        Task<(RepositoryEvent, MovieModel)> CreateMovie(Movie newMovie);
        Task<RepositoryEvent> UpdateMovie(Movie movie);
        Task<IEnumerable<Movie>> GetAllMovies();
        Task<bool> DeleteMovie(int id);
    }
}
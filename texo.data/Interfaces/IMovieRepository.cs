using System.Collections.Generic;
using System.Threading.Tasks;
using texo.commons.Interfaces;
using texo.data.Entities;

namespace texo.data.Interfaces
{
    public interface IMovieRepository : IRepository<MovieModel>
    {
        
        Task<IEnumerable<MovieModel>> GetWinnerMoviesFromProducer(int producerId);
        Task<IEnumerable<MovieModel>> GetWinnerMovies();
        
    }
}
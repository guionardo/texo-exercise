using System.Collections.Generic;
using System.Threading.Tasks;
using texo.data.Entities;

namespace texo.data.Interfaces
{
    public interface IAssociationMovieProducersRepository
    {
        Task Assign(MovieModel parent, IEnumerable<ProducerModel> children);
        Task<IEnumerable<ProducerModel>> GetProducersFromMovie(int movieId);
    }
}
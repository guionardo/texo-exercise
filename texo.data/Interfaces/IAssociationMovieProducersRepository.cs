using System.Collections.Generic;
using System.Threading.Tasks;
using texo.domain.Entities;
using Movie = texo.data.Entities.Movie;

namespace texo.data.Interfaces
{
    public interface IAssociationMovieProducersRepository
    {
        Task Assign(Movie parent, IEnumerable<Producer> children);
    }
}
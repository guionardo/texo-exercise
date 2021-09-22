using System.Collections.Generic;
using System.Threading.Tasks;
using texo.domain.Entities;
using Movie = texo.data.Entities.Movie;

namespace texo.data.Interfaces
{
    public interface IAssociationMovieStudiosRepository
    {
        Task Assign(Movie parent, IEnumerable<Studio> children);
    }
}
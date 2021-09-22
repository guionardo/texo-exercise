using System.Collections.Generic;
using System.Threading.Tasks;
using texo.data.Entities;

namespace texo.data.Interfaces
{
    public interface IAssociationMovieStudiosRepository
    {
        Task Assign(MovieModel parent, IEnumerable<StudioModel> children);
        
        Task<IEnumerable<StudioModel>> GetStudiosFromMovie(int movieId);
    }
}
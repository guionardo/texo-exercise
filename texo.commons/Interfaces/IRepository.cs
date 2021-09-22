using System.Threading.Tasks;

namespace texo.commons.Interfaces
{
    public interface IRepository<T> where T : ITexoEntity
    {
        Task<T> Get(int id);
        Task Set(T entity);

        Task<T> FindText(string text);
    }
}
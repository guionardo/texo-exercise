using System.Threading.Tasks;

namespace texo.data.Interfaces
{
    public interface IAssociationRepository
    {
        Task UnassignAll(int id);
    }
}
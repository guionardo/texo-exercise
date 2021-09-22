using System.Threading.Tasks;

namespace texo.data.Interfaces
{
    public interface IAggregatorService
    {
        /// <summary>
        /// Read CSV file and populates data into tables
        /// </summary>
        Task LoadDataFromCsv();
    }
}
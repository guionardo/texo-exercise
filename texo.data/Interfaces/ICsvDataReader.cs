using System.Collections.Generic;
using texo.data.Dtos;

namespace texo.data.Interfaces
{
    public interface ICsvDataReader
    {
        IEnumerable<CsvMovieDto> GetMovies(string fileName);
    }
}
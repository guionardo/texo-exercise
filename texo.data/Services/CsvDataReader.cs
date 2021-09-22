using System.Collections.Generic;
using System.IO;
using System.Linq;
using texo.data.Dtos;
using texo.data.Interfaces;

namespace texo.data.Services
{
    public class CsvDataReader : ICsvDataReader
    {
        public IEnumerable<CsvMovieDto> GetMovies(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Source file not found", fileName);

            return File.ReadLines(fileName).Skip(1).Select(ParseCsvLine).ToArray();
        }

        private static CsvMovieDto ParseCsvLine(string line)
        {
            // 1988;Caddyshack II;Warner Bros.;Neil Canton, Jon Peters and Peter Guber;

            var words = line.Split(';');
            return new CsvMovieDto
            {
                Year = int.Parse(words[0]),
                Title = words[1],
                Studios = words[2],
                Producers = words[3],
                Winner = words.Length > 4 ? words[4] : ""
            };
        }
    }
}
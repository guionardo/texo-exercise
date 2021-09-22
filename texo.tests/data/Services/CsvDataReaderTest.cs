using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using texo.data.Services;
using Xunit;

namespace texo.tests.data.Services
{
    [ExcludeFromCodeCoverage]
    public class CsvDataReaderTest
    {
        private const string Sample = @"year;title;studios;producers;winner
1980;Can't Stop the Music;Associated Film Distribution;Allan Carr;yes
1980;Cruising;Lorimar Productions, United Artists;Jerry Weintraub;
1980;The Formula;MGM, United Artists;Steve Shagan;";

        [Fact]
        public void TestCsvReader()
        {
            var csvFile = Path.GetTempFileName();
            File.WriteAllText(csvFile, Sample);
            var csvReader = new CsvDataReader();
            var movies = csvReader.GetMovies(csvFile).ToArray();
            File.Delete(csvFile);

            Assert.Equal(3, movies.Length);
            Assert.Equal(1980, movies[0].Year);
            Assert.Equal("Cruising", movies[1].Title);
            Assert.Equal("Jerry Weintraub", movies[1].Producers);
            Assert.Equal("yes", movies[0].Winner);
            Assert.Equal("MGM, United Artists", movies[2].Studios);
        }

        [Fact]
        public void TestInexistentFile()
        {
            Assert.Throws<FileNotFoundException>(() =>
            {
                var cvsReader = new CsvDataReader();
                cvsReader.GetMovies("inexistent_file.csv");
            });
        }
    }
}
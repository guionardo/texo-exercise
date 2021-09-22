using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using texo.data.Dtos;
using texo.data.Entities;
using texo.data.Interfaces;

namespace texo.data.Services
{
    public class AggregatorService : IAggregatorService
    {
        private readonly ILogger<AggregatorService> _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly IStudioRepository _studioRepository;
        private readonly ICsvDataReader _csvDataReader;
        private readonly IAssociationMovieStudiosRepository _associationMovieStudiosRepository;
        private readonly IAssociationMovieProducersRepository _associationMovieProducersRepository;
        private readonly string _sourceFile;

        public AggregatorService(ILogger<AggregatorService> logger, IMovieRepository movieRepository,
            IProducerRepository producerRepository, IStudioRepository studioRepository, ICsvDataReader csvDataReader,
            IAssociationMovieStudiosRepository associationMovieStudiosRepository,
            IAssociationMovieProducersRepository associationMovieProducersRepository,
            IConfiguration configuration)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _producerRepository = producerRepository;
            _studioRepository = studioRepository;
            _csvDataReader = csvDataReader;
            _associationMovieStudiosRepository = associationMovieStudiosRepository;
            _associationMovieProducersRepository = associationMovieProducersRepository;
            _sourceFile = configuration.GetConnectionString("source_file");
            if (string.IsNullOrEmpty(_sourceFile))
                throw new ArgumentException("Missing environment source_file");
            if (!File.Exists(_sourceFile))
                throw new FileNotFoundException("Missing source file", _sourceFile);
        }

        public async Task LoadDataFromCsv()
        {
            var movies = _csvDataReader.GetMovies(_sourceFile).ToArray();
            foreach (var movieDto in movies)
            {
                var movie = await ReadMovie(movieDto);
                var producers = await ReadProducers(movieDto);
                var studios = await ReadStudios(movieDto);
                await AssignProducersToMovie(movie, producers);
                await AssignStudiosToMovie(movie, studios);
            }
        }

        private async Task AssignStudiosToMovie(MovieModel movieModel, IEnumerable<StudioModel> studios)
        {
            await _associationMovieStudiosRepository.Assign(movieModel, studios);
        }

        private async Task AssignProducersToMovie(MovieModel movieModel, IEnumerable<ProducerModel> producers)
        {
            await _associationMovieProducersRepository.Assign(movieModel, producers);
        }

        private async Task<IEnumerable<StudioModel>> ReadStudios(CsvMovieDto movieDto)
        {
            var studiosNames = movieDto.Studios.Split(',').Select(s => s.Trim()).ToArray();
            var studios = new List<StudioModel>();
            foreach (var studioName in studiosNames)
            {
                var studio = await _studioRepository.FindText(studioName);
                if (studio is null)
                {
                    studio = new StudioModel
                    {
                        Id = 0,
                        Name = studioName
                    };
                    await _studioRepository.Set(studio);
                }

                studios.Add(studio);
            }

            return studios;
        }

        private async Task<IEnumerable<ProducerModel>> ReadProducers(CsvMovieDto movieDto)
        {
            var producerNames = movieDto.Producers.Replace(" and ", ",").Split(',').Select(p => p.Trim()).ToArray();
            var producers = new List<ProducerModel>();
            foreach (var producerName in producerNames)
            {
                var producer = await _producerRepository.FindText(producerName);
                if (producer is null)
                {
                    producer = new ProducerModel
                    {
                        Id = 0,
                        Name = producerName
                    };
                    await _producerRepository.Set(producer);
                }

                producers.Add(producer);
            }

            return producers;
        }

        private async Task<MovieModel> ReadMovie(CsvMovieDto movieDto)
        {
            var movie = await _movieRepository.FindText(movieDto.Title);
            if (movie is not null) return movie;
            movie = new MovieModel
            {
                Id = 0,
                Title = movieDto.Title,
                Winner = !string.IsNullOrEmpty(movieDto.Winner),
                Release = movieDto.Year
            };
            await _movieRepository.Set(movie);

            return movie;
        }
    }
}
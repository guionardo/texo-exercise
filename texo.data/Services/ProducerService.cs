using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using texo.data.Dtos;
using texo.data.Entities;
using texo.data.Interfaces;
using texo.domain.Entities;

namespace texo.data.Services
{
    public class ProducerService : IProducerService
    {
        private readonly ILogger<ProducerService> _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly IAssociationMovieProducersRepository _movieProducersRepository;
        private readonly IAssociationMovieStudiosRepository _movieStudiosRepository;
        private readonly IProducerRepository _producerRepository;

        public ProducerService(ILogger<ProducerService> logger, IMovieRepository movieRepository,
            IAssociationMovieProducersRepository movieProducersRepository,
            IAssociationMovieStudiosRepository movieStudiosRepository,
            IProducerRepository producerRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _movieProducersRepository = movieProducersRepository;
            _movieStudiosRepository = movieStudiosRepository;
            _producerRepository = producerRepository;
        }


        public async Task<IEnumerable<Movie>> GetWinnersFromProducer(int producerId)
        {
            var movies = await _movieRepository.GetWinnerMoviesFromProducer(producerId);
            return await GetMovies(movies);
        }

        public async Task<IEnumerable<Movie>> GetWinners()
        {
            var movies = await _movieRepository.GetWinnerMovies();
            return await GetMovies(movies);
        }


        public async Task<IEnumerable<ProducerWinnerIntervalDto>> GetWiningsIntervalsFromProducer(int producerId)
        {
            var movies = (await GetWinnersFromProducer(producerId)).ToArray();
            if (movies.Length < 2)
                return Array.Empty<ProducerWinnerIntervalDto>();
            var producer = await _producerRepository.Get(producerId);
            Movie last = null;
            var intervals = new List<ProducerWinnerIntervalDto>();
            foreach (var movie in movies)
            {
                if (last != null)
                {
                    intervals.Add(new ProducerWinnerIntervalDto
                    {
                        Producer = producer.Name,
                        PreviousWin = last.Release,
                        FollowingWin = movie.Release
                    });
                }

                last = movie;
            }

            return intervals;
        }

        public async Task<IEnumerable<ProducerWinnerIntervalDto>> GetWiningsIntervals()
        {
            var producers = await _producerRepository.GetAll();
            var allIntervals = new List<ProducerWinnerIntervalDto>();
            foreach (var producer in producers)
            {
                var intervals = (await GetWiningsIntervalsFromProducer(producer.Id)).ToImmutableArray();
                if (intervals.Any())
                    allIntervals.AddRange(intervals);
            }

            return allIntervals;
        }

        public async Task<(IEnumerable<ProducerWinnerIntervalDto>, IEnumerable<ProducerWinnerIntervalDto>)>
            GetFirstAndLastWinProducers()
        {
            var winnerProducers = await _producerRepository.GetWinnerProducers();
            var winnerIntervals = new List<ProducerWinnerIntervalDto>();
            foreach (var producerId in winnerProducers)
            {
                var intervals = await _producerRepository.GetWinnerIntervalsFromProducer(producerId);
                winnerIntervals.AddRange(intervals);
            }

            winnerIntervals.Sort((a, b) => a.Interval.CompareTo(b.Interval));
            var less = winnerIntervals.First().Interval;
            var more = winnerIntervals.Last().Interval;
            return (
                winnerIntervals.Where(w => w.Interval == less).ToArray(),
                winnerIntervals.Where(w => w.Interval == more).ToArray());
        }

        private async Task<IEnumerable<Movie>> GetMovies(IEnumerable<MovieModel> movies)
        {
            var moviesResult = new List<Movie>();
            foreach (var movie in movies)
            {
                moviesResult.Add(await GetMovie(movie));
            }

            return moviesResult;
        }

        private async Task<Movie> GetMovie(MovieModel movie)
        {
            var movieResult = new Movie
            {
                Id = movie.Id,
                Title = movie.Title,
                Winner = movie.Winner,
                Release = movie.Release,
                Producers = await _movieProducersRepository.GetProducersFromMovie(movie.Id),
                Studios = await _movieStudiosRepository.GetStudiosFromMovie(movie.Id)
            };
            return movieResult;
        }
    }
}
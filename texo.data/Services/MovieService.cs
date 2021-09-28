using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using texo.data.Entities;
using texo.data.Enums;
using texo.data.Interfaces;
using texo.domain.Entities;

namespace texo.data.Services
{
    public class MovieService : IMovieService
    {
        private readonly ILogger<MovieService> _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly IStudioRepository _studioRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly IAssociationMovieProducersRepository _movieProducersRepository;
        private readonly IAssociationMovieStudiosRepository _movieStudiosRepository;

        public MovieService(ILogger<MovieService> logger,
            IMovieRepository movieRepository,
            IStudioRepository studioRepository,
            IProducerRepository producerRepository,
            IAssociationMovieProducersRepository movieProducersRepository,
            IAssociationMovieStudiosRepository movieStudiosRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _studioRepository = studioRepository;
            _producerRepository = producerRepository;
            _movieProducersRepository = movieProducersRepository;
            _movieStudiosRepository = movieStudiosRepository;
        }

        public async Task<Movie> GetMovieById(int id)
        {
            var movie = await _movieRepository.Get(id);
            if (movie is null)
                return null;
            return await GetAggregateMovie(movie);
        }

        private async Task<Movie> GetAggregateMovie(MovieModel movie)
        {
            var studios = await _movieStudiosRepository.GetStudiosFromMovie(movie.Id);
            var producers = await _movieProducersRepository.GetProducersFromMovie(movie.Id);
            return new Movie
            {
                Id = movie.Id,
                Title = movie.Title,
                Release = movie.Release,
                Winner = movie.Winner,
                Producers = producers.Select(p => new Producer { Id = p.Id, Name = p.Name }).ToArray(),
                Studios = studios.Select(s => new Studio { Id = s.Id, Name = s.Name }).ToArray()
            };
        }

        public async Task<(RepositoryEvent, MovieModel)> CreateMovie(Movie newMovie)
        {
            var movie = await _movieRepository.FindText(newMovie.Title);
            if (movie is not null)
                return (RepositoryEvent.PrimaryKeyFail, movie);
            movie = new MovieModel
            {
                Id = 0,
                Release = newMovie.Release,
                Title = newMovie.Title,
                Winner = newMovie.Winner
            };
            if (await SaveMovie(newMovie, movie))
                return (RepositoryEvent.Created, movie);
            return (RepositoryEvent.UpdateFail, movie);
        }

        public async Task<RepositoryEvent> UpdateMovie(Movie newMovie)
        {
            var movie = new MovieModel
            {
                Id = newMovie.Id,
                Release = newMovie.Release,
                Title = newMovie.Title,
                Winner = newMovie.Winner
            };
            if (await SaveMovie(newMovie, movie))
                return RepositoryEvent.Updated;

            return RepositoryEvent.UpdateFail;
        }

        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            var movies = new List<Movie>();
            foreach (var movie in await _movieRepository.GetAll())
            {
                movies.Add(await GetAggregateMovie(movie));
            }

            return movies;
        }

        public async Task<bool> DeleteMovie(int id)
        {
            try
            {
                await _movieProducersRepository.UnassignAll(id);
                await _movieStudiosRepository.UnassignAll(id);
                await _movieRepository.Delete(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> SaveMovie(Movie newMovie, MovieModel movie)
        {
            try
            {
                await _movieRepository.Set(movie);
                await ProcessStudios(newMovie.Studios);
                await ProcessProducers(newMovie.Producers);
                await _movieStudiosRepository.Assign(movie,
                    newMovie.Studios.Select(s => new StudioModel { Id = s.Id, Name = s.Name }));
                await _movieProducersRepository.Assign(movie,
                    newMovie.Producers.Select(p => new ProducerModel { Id = p.Id, Name = p.Name }));
                return true;
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Failed to save movie");
            }

            return false;
        }

        private async Task ProcessProducers(IEnumerable<Producer> newMovieProducers)
        {
            foreach (var producer in newMovieProducers)
            {
                var producerModel = new ProducerModel
                {
                    Id = producer.Id < 1 ? 0 : producer.Id,
                    Name = producer.Name
                };
                await _producerRepository.Set(producerModel);
                producer.Id = producerModel.Id;
            }
        }

        private async Task ProcessStudios(IEnumerable<Studio> newMovieStudios)
        {
            foreach (var studio in newMovieStudios)
            {
                var studioModel = new StudioModel
                {
                    Id = studio.Id < 1 ? 0 : studio.Id,
                    Name = studio.Name
                };

                await _studioRepository.Set(studioModel);
                studio.Id = studioModel.Id;
            }
        }
    }
}
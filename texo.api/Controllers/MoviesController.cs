using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using texo.api.Requests;
using texo.data.Enums;
using texo.data.Interfaces;
using texo.domain.Entities;

namespace texo.api.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Movie>> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieById(id);
            return movie is null ? NotFound() : Ok(movie);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Movie>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            var movies = await _movieService.GetAllMovies();
            return movies is null || !movies.Any() ? NotFound() : Ok(movies);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Movie))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Movie>> PostNewMovie([FromBody] MovieRequest request)
        {
            var newMovie = new Movie
            {
                Id = 0,
                Release = request.Release,
                Title = request.Title,
                Winner = request.Winner,
                Producers = request.Producers.Select(p => new Producer { Id = 0, Name = p }).ToArray(),
                Studios = request.Studios.Select(s => new Studio { Id = 0, Name = s }).ToArray()
            };
            var (postResult, movie) = await _movieService.CreateMovie(newMovie);
            return postResult switch
            {
                RepositoryEvent.Created => Created($"movies/{movie.Id}", movie),
                RepositoryEvent.PrimaryKeyFail => Conflict("Movie existent"),
                _ => throw new ArgumentOutOfRangeException(nameof(postResult))
            };
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutMovie([FromBody] MovieRequest request)
        {
            if (!request.Id.HasValue)
                return BadRequest();
            var movie = await _movieService.GetMovieById(request.Id.Value);
            if (movie is null)
                return NotFound();
            movie.Title = request.Title;
            movie.Release = request.Release;
            movie.Winner = request.Winner;
            movie.Producers = request.Producers.Select(p => new Producer { Id = 0, Name = p }).ToArray();
            movie.Studios = request.Studios.Select(s => new Studio { Id = 0, Name = s }).ToArray();
            var postResult = await _movieService.UpdateMovie(movie);
            return postResult switch
            {
                RepositoryEvent.Updated => Accepted($"movies/{movie.Id}", movie),
                _ => throw new ArgumentOutOfRangeException(nameof(postResult))
            };
        }

        /// <summary>
        /// Delete movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteMovie(int id)
        {
            var success = await _movieService.DeleteMovie(id);
            return success ? Accepted() : NotFound();
        }
    }
}
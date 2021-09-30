using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using texo.api.Controllers;
using texo.api.Requests;
using texo.data.Interfaces;
using texo.domain.Entities;
using Xunit;

namespace texo.tests.api.Controllers
{
    [ExcludeFromCodeCoverage]
    public class Movies
    {
        private readonly MoviesController _moviesController;

        public Movies()
        {
            ServicesAux.SetupDatabase();
            var provider = ServicesAux.GetServiceProvider();
            var movieService = provider.GetService<IMovieService>();
            _moviesController = new MoviesController(movieService);
        }

        [Fact]
        public async Task TestMoviesController_GetMovieById()
        {
            var result = await _moviesController.GetMovieById(1);
            Assert.Equal(200, (result.Result as OkObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestMoviesController_GetAllMovies()
        {
            var result = await _moviesController.GetAllMovies();
            Assert.Equal(200, (result.Result as OkObjectResult).StatusCode);
        }

        [Fact]
        public async Task TestMoviesController_PostNewMovie()
        {
            var newMovie = new MovieRequest
            {
                Release = 2021,
                Title = "Testing new movie",
                Producers = new[] { "Guionardo Furlan" },
                Studios = new[] { "Guiosoft" },
                Winner = false
            };
            var result = await _moviesController.PostNewMovie(newMovie);
            Assert.Equal(201, (result.Result as CreatedResult).StatusCode);
        }

        [Fact]
        public async Task TestMoviesController_PutMovie()
        {
            var movieResult = (await _moviesController.GetMovieById(1)).Result as OkObjectResult;
            var existingMovie = movieResult.Value as Movie;
            
            var movie = new MovieRequest
            {
                Id=existingMovie.Id,
                Release = existingMovie.Release-1,
                Title = existingMovie.Title,
                Producers = new[] { "Guionardo Furlan" },
                Studios = new[] { "Guiosoft" },
                Winner = false
            };
            var result = await _moviesController.PutMovie(movie);
            
            Assert.Equal(202, (result as AcceptedResult).StatusCode);

            movie.Id = 909090;
            result = await _moviesController.PutMovie(movie);
            Assert.Equal(404,(result as NotFoundResult).StatusCode);

            movie.Id = null;
            result = await _moviesController.PutMovie(movie);
            Assert.Equal(400,(result as BadRequestResult).StatusCode);
        }

        [Fact]
        public async Task TestMoviesController_DeleteMovie()
        {
            var result = await _moviesController.DeleteMovie(10);
            Assert.Equal(202, (result as AcceptedResult).StatusCode);

            result = await _moviesController.DeleteMovie(9999999);
            Assert.Equal(404,(result as NotFoundResult).StatusCode);
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using texo.data.Abstractions;
using texo.data.Entities;
using texo.data.Extensions;
using texo.data.Interfaces;

namespace texo.data.Repositories
{
    public class AssociationMovieStudioRepository : AssociationAbstractRepository<MovieModel, StudioModel,MovieStudioModel>,
        IAssociationMovieStudiosRepository

    {
        public AssociationMovieStudioRepository(
            ILogger<AssociationMovieStudioRepository> logger,
            IDatabaseBootstrap databaseBootstrap
        ) : base(logger, databaseBootstrap,
            nameof(MovieStudioModel.IdMovie).GetAsSnakeCase(),
            nameof(MovieStudioModel.IdStudio).GetAsSnakeCase())
        {
        }

        public async Task<IEnumerable<StudioModel>> GetStudiosFromMovie(int movieId)
        {
            await using var db = DatabaseBootstrap.GetConnection();
            var studios = await db.QueryAsync<StudioModel>(
                "SELECT s.* FROM movie_studios mp LEFT JOIN studios s on s.id==mp.id_studio WHERE mp.id_movie = @Id",
                new { Id = movieId });
            return studios;
        }
    }
}
using Microsoft.Extensions.Logging;
using texo.data.Abstractions;
using texo.data.Entities;
using texo.data.Interfaces;
using texo.domain.Entities;

namespace texo.data.Repositories
{
    public class StudioRepository : AbstractRepository<StudioModel>, IStudioRepository
    {
        public StudioRepository(ILogger<StudioRepository> logger, IDatabaseBootstrap databaseBootstrap) : base(logger,
            databaseBootstrap,
            nameof(Studio.Name), studio => studio.Name)
        {
        }
    }
}
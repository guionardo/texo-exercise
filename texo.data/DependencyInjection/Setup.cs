using Microsoft.Extensions.DependencyInjection;
using texo.data.Interfaces;
using texo.data.Repositories;
using texo.data.Services;

namespace texo.data.DependencyInjection
{
    public static class Setup
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services
                .AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>()
                .AddSingleton<ICsvDataReader, CsvDataReader>()
                .AddScoped<IAggregatorService, AggregatorService>()
                .AddScoped<IMovieRepository, MovieRepository>()
                .AddScoped<IProducerRepository, ProducerRepository>()
                .AddScoped<IStudioRepository, StudioRepository>()
                .AddScoped<IAssociationMovieStudiosRepository, AssociationMovieStudioRepository>()
                .AddScoped<IAssociationMovieProducersRepository, AssociationMovieProducersRepository>()
                .AddScoped<IProducerService, ProducerService>();
        }
    }
}
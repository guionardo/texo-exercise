using Dapper.Contrib.Extensions;
using texo.commons.Interfaces;

namespace texo.data.Entities
{
    [Table("movie_studios")]
    public abstract class MovieStudioModel : ITexoEntity
    {
        public int IdMovie { get; set; }
        public int IdStudio { get; set; }
        public int Id { get; set; }
    }
}
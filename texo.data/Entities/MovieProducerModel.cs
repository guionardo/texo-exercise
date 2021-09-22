using Dapper.Contrib.Extensions;
using texo.commons.Interfaces;

namespace texo.data.Entities
{
    [Table("movie_producers")]
    public class MovieProducerModel : ITexoEntity
    {
        public int IdMovie { get; set; }
        public int IdProducer { get; set; }
        public int Id { get; set; }
    }
}
using texo.commons.Interfaces;

namespace texo.data.Entities
{
    [Dapper.Contrib.Extensions.Table("movies")]
    public class MovieModel : ITexoEntity
    {
        public int Id { get; set; }
        public int Release { get; set; }
        public string Title { get; set; }
        public bool Winner { get; set; }
    }
}
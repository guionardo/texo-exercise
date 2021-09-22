using texo.commons.Interfaces;

namespace texo.data.Entities
{
    public class Movie : ITexoEntity
    {
        public int Id { get; set; }
        public int ReleaseYear { get; set; }
        public string Title { get; set; }
        public bool Winner { get; set; }
    }
}
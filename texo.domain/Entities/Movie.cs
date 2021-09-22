using System.Collections.Generic;
using texo.commons.Interfaces;


namespace texo.domain.Entities
{
    public class Movie : ITexoEntity
    {
        public int Id { get; set; }
        public int ReleaseYear { get; set; }
        public string Title { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
        public IEnumerable<Producer> Producers { get; set; }
        public bool Winner { get; set; }
    }
}
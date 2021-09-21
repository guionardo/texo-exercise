using System.Collections.Generic;

namespace texo.domain.Entities
{
    public class Movie
    {
        public int Year { get; set; }
        public string Title { get; set; }
        public string Studios { get; set; }
        public IEnumerable<Producer> Producers { get; set; }
        public bool Winner { get; set; }
    }
}
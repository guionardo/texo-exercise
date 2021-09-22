using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using texo.commons.Interfaces;


namespace texo.domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class Movie : ITexoEntity
    {
        public int Id { get; set; }
        public int Release { get; set; }
        public string Title { get; set; }
        public IEnumerable<Studio> Studios { get; set; }
        public IEnumerable<Producer> Producers { get; set; }
        public bool Winner { get; set; }
    }
}
using System.Diagnostics.CodeAnalysis;
using texo.commons.Interfaces;

namespace texo.domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class Studio : ITexoEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
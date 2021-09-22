using System.Diagnostics.CodeAnalysis;
using texo.commons.Interfaces;

namespace texo.domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class Producer : ITexoEntity
    {
        public int Id { get; set; }
        public string Name { get; init; }
    }
}
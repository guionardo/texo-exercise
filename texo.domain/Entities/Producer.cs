using texo.commons.Interfaces;

namespace texo.domain.Entities
{
    public class Producer : ITexoEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
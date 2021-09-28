using System;

namespace texo.data.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string tableName, int entityId) : base(
            $"Entity not found {tableName} # {entityId}")
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using texo.commons.Interfaces;
using texo.data.Interfaces;

namespace texo.data.Abstractions
{
    public class AssociationAbstractRepository<TParent, TChild> where TParent : ITexoEntity where TChild : ITexoEntity
    {
        private readonly ILogger _logger;
        private readonly IDatabaseBootstrap _databaseBootstrap;
        private readonly string _parentKeyField;
        private readonly string _childKeyField;


        protected AssociationAbstractRepository(ILogger logger, IDatabaseBootstrap databaseBootstrap,
            string parentKeyField, string childKeyField)
        {
            _logger = logger;
            _databaseBootstrap = databaseBootstrap;
            _parentKeyField = parentKeyField;
            _childKeyField = childKeyField;
        }

        public async Task Assign(TParent parent, IEnumerable<TChild> children)
        {
            _logger.LogInformation("Assigning {0} - {1}", typeof(TParent).Name, typeof(TChild).Name);
            var parentTable = typeof(TParent).Name.ToLower();
            var childTable = typeof(TChild).Name.ToLower();
            try
            {
                var deleteCommand = new CommandDefinition(
                    $"DELETE FROM {parentTable}_{childTable}s WHERE {_parentKeyField}=@Id",
                    new { parent.Id });

                await using var db = _databaseBootstrap.GetConnection();
                var deletedCount = await db.ExecuteAsync(deleteCommand);
                if (deletedCount > 0)
                    _logger.LogInformation("Removed {0} previous assignments", deletedCount);

                var inserted = 0;
                foreach (var child in children)
                {
                    var insertCommand = new CommandDefinition(
                        $"INSERT INTO {parentTable}_{childTable}s ({_parentKeyField},{_childKeyField}) VALUES (@P,@C)",
                        new { P = parent.Id, C = child.Id });
                    inserted += await db.ExecuteAsync(insertCommand);
                }

                _logger.LogInformation("Assigned {0} - {1} - {2}/{3}", typeof(TParent).Name, typeof(TChild).Name,
                    inserted,
                    children.Count());
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Failed to assign");
                throw;
            }
        }
    }
}
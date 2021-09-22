using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using texo.commons.Interfaces;
using texo.data.Extensions;
using texo.data.Interfaces;

namespace texo.data.Abstractions
{
    public class AssociationAbstractRepository<TParent, TChild, TModel>
        where TParent : ITexoEntity
        where TChild : ITexoEntity
        where TModel : ITexoEntity
    {
        private readonly ILogger _logger;
        protected readonly IDatabaseBootstrap DatabaseBootstrap;
        private readonly string _parentKeyField;
        private readonly string _childKeyField;
        private readonly string _associationTableName;


        protected AssociationAbstractRepository(ILogger logger, IDatabaseBootstrap databaseBootstrap,
            string parentKeyField, string childKeyField)
        {
            _logger = logger;
            DatabaseBootstrap = databaseBootstrap;
            _associationTableName = typeof(TModel).GetTableName();
            _parentKeyField = parentKeyField;
            _childKeyField = childKeyField;
        }

        public async Task Assign(TParent parent, IEnumerable<TChild> children)
        {
            _logger.LogInformation("Assigning {0} - {1}", typeof(TParent).Name, typeof(TChild).Name);
            try
            {
                var deleteCommand = new CommandDefinition(
                    $"DELETE FROM {_associationTableName} WHERE {_parentKeyField}=@Id",
                    new { parent.Id });

                await using var db = DatabaseBootstrap.GetConnection();
                var deletedCount = await db.ExecuteAsync(deleteCommand);
                if (deletedCount > 0)
                    _logger.LogInformation("Removed {0} previous assignments", deletedCount);

                var inserted = 0;
                var texoEntities = children as TChild[] ?? children.ToArray();
                foreach (var child in texoEntities)
                {
                    var insertCommand = new CommandDefinition(
                        $"INSERT INTO {_associationTableName} ({_parentKeyField},{_childKeyField}) VALUES (@P,@C)",
                        new { P = parent.Id, C = child.Id });
                    inserted += await db.ExecuteAsync(insertCommand);
                }

                _logger.LogInformation("Assigned {0} - {1} - {2}/{3}", typeof(TParent).Name, typeof(TChild).Name,
                    inserted,
                    texoEntities.Length);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Failed to assign");
                throw;
            }
        }
    }
}
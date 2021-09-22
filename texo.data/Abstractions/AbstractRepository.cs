using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using texo.commons.Interfaces;
using texo.data.Extensions;
using texo.data.Interfaces;

namespace texo.data.Abstractions
{
    public abstract class AbstractRepository<T> : IRepository<T> where T : ITexoEntity
    {
        private readonly ILogger _logger;
        protected readonly IDatabaseBootstrap DatabaseBootstrap;
        private readonly Func<T, string> _getTextFieldValueMethod;
        private readonly string _textFieldName;
        private readonly string _tableName;

        protected AbstractRepository(ILogger logger, IDatabaseBootstrap databaseBootstrap, string textFieldName,
            Func<T, string> getTextFieldValueMethod)
        {
            _logger = logger;
            DatabaseBootstrap = databaseBootstrap;
            _getTextFieldValueMethod = getTextFieldValueMethod;
            _textFieldName = textFieldName.GetAsSnakeCase();
            _tableName = typeof(T).GetTableName();
        }

        public async Task<T> Get(int id)
        {
            await using var db = DatabaseBootstrap.GetConnection();
            return await db.QueryFirstOrDefaultAsync<T>($"select * from {_tableName} where id={id};");
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            await using var db = DatabaseBootstrap.GetConnection();
            return await db.QueryAsync<T>($"select * from {_tableName};");
        }

        public async Task Set(T entity)
        {
            await using var db = DatabaseBootstrap.GetConnection();
            var sql = entity.Id < 1 ? GetInsertSql(entity) : GetUpdateSql(entity);

            try
            {
                var rowsAffected = await db.ExecuteAsync(sql, entity);
                if (entity.Id < 1)
                {
                    if (rowsAffected > 0)
                    {
                        var newEntity = await FindText(_getTextFieldValueMethod(entity));
                        if (newEntity is null)
                        {
                            _logger.LogWarning("Failed to insert data @ {0}", _tableName);
                        }
                        else
                        {
                            entity.Id = newEntity.Id;
                        }
                    }

                    _logger.LogInformation("Insert into {0} = {1}", _tableName, entity.Id);
                }
                else
                {
                    _logger.LogInformation("Update {0} #{1} = {2}", _tableName, entity.Id, rowsAffected);
                }
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Failed to upsert data {0} {1}", _tableName, exc.Message);
            }
        }

        private string GetUpdateSql(T entity)
        {
            var fields = string.Join(',', entity.GetFieldNames());
            var fieldsArgs = string.Join(',', entity.GetFieldNames(false, "@"));
            return $"UPDATE {_tableName} ({fields}) VALUES ({fieldsArgs})";
        }

        private string GetInsertSql(T entity)
        {
            var fields = string.Join(',', entity.GetFieldNames(true));
            var fieldsArgs = string.Join(',', entity.GetFieldNames(true, "@", true));
            return $"INSERT INTO {_tableName} ({fields}) VALUES ({fieldsArgs});";
        }

        public async Task<T> FindText(string text)
        {
            await using var db = DatabaseBootstrap.GetConnection();
            var parameter = new
            {
                Text = text
            };
            var command = new CommandDefinition($"SELECT * FROM {_tableName} WHERE {_textFieldName}=@Text", parameter);
            try
            {
                var result = await db.QueryFirstOrDefaultAsync<T>(command);
                if (result is null)
                {
                    _logger.LogInformation("FindText {0} ({1}) NOT FOUND", _tableName, text);
                }
                else
                {
                    _logger.LogInformation("FindText {0} ({1}) = {2}", _tableName, text, result.Id);
                }

                return result;
            }
            catch (Exception exc)
            {
                _logger.LogError(exc, "Failed to find text {0} ({1}) {3}", _tableName, text, exc.Message);
                throw;
            }
        }
    }
}
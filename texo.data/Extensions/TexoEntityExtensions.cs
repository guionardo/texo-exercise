using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper.Contrib.Extensions;
using texo.commons.Interfaces;
using texo.data.Exceptions;

namespace texo.data.Extensions
{
    public static class TexoEntityExtensions
    {
        public static IEnumerable<string> GetFieldNames(this ITexoEntity entity, bool withoutId = false,
            string prefix = "", bool noSnakeCase = false)
        {
            var members = entity.GetType().GetMembers()
                .Where(m => m.MemberType == MemberTypes.Property).ToList();
            return (from member in members
                where !withoutId || member.Name.ToLower() != "id"
                select prefix + (noSnakeCase ? member.Name : member.Name.GetAsSnakeCase())).ToArray();
        }

        public static string GetAsSnakeCase(this string text)
        {
            return string.Concat(
                text.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString().ToLower() : x.ToString().ToLower())
            );
        }

        internal static string GetTableName(this MemberInfo t)
        {
            var tableAttr = (TableAttribute)Attribute.GetCustomAttribute(t, typeof(TableAttribute));
            if (tableAttr is null)
                throw new MissingConfigurationException($"Type {t.Name} doesn't have Table attribute");
            return tableAttr.Name;
        }
       
    }
}
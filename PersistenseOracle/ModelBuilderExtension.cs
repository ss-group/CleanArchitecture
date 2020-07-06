using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PersistenseOracle
{
    public static class ModelBuilderExtension
    {
        public static string ToUpperNamingConvention(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToUpper();
        }

        public static IEnumerable<IMutableEntityType> EntityTypes(this ModelBuilder builder)
        {
            return builder.Model.GetEntityTypes();
        }

        public static IEnumerable<IMutableProperty> Properties(this ModelBuilder builder)
        {
            return builder.EntityTypes().SelectMany(entityType => entityType.GetProperties());
        }

        public static IEnumerable<IMutableProperty> Properties<T>(this ModelBuilder builder)
        {
            return builder.EntityTypes().SelectMany(entityType => entityType.GetProperties().Where(x => x.ClrType == typeof(T)));
        }

        public static void Configure(this IEnumerable<IMutableEntityType> entityTypes, Action<IMutableEntityType> convention)
        {
            foreach (var entityType in entityTypes)
            {
                convention(entityType);
            }
        }

        public static void Configure(this IEnumerable<IMutableProperty> propertyTypes, Action<IMutableProperty> convention)
        {
            foreach (var propertyType in propertyTypes)
            {
                convention(propertyType);
            }
        }
    }
}

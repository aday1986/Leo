using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Leo.ThirdParty.AutoMapper.Configuration;
using Leo.ThirdParty.AutoMapper.Mappers.Internal;

namespace Leo.ThirdParty.AutoMapper.Mappers
{
    using static CollectionMapperExpressionFactory;

    public class HashSetMapper : IObjectMapper
    {
        public bool IsMatch(TypePair context)
            => context.SourceType.IsEnumerableType() && IsSetType(context.DestinationType);

        public Expression MapExpression(IConfigurationProvider configurationProvider, ProfileMap profileMap,
            IMemberMap memberMap, Expression sourceExpression, Expression destExpression, Expression contextExpression)
            => MapCollectionExpression(configurationProvider, profileMap, memberMap, sourceExpression, destExpression, contextExpression, typeof(HashSet<>), MapItemExpr);

        private static bool IsSetType(Type type) => type.IsSetType();
    }
}
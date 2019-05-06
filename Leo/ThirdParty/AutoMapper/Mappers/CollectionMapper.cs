using System.Collections.Generic;
using System.Linq.Expressions;
using Leo.ThirdParty.AutoMapper.Configuration;
using Leo.ThirdParty.AutoMapper.Mappers.Internal;

namespace Leo.ThirdParty.AutoMapper.Mappers
{
    using static CollectionMapperExpressionFactory;

    public class CollectionMapper : EnumerableMapperBase
    {
        public override bool IsMatch(TypePair context) => context.SourceType.IsEnumerableType() && context.DestinationType.IsCollectionType();

        public override Expression MapExpression(IConfigurationProvider configurationProvider, ProfileMap profileMap,
            IMemberMap memberMap, Expression sourceExpression, Expression destExpression, Expression contextExpression)
            => MapCollectionExpression(configurationProvider, profileMap, memberMap, sourceExpression, destExpression, contextExpression, typeof(List<>), MapItemExpr);
    }
}
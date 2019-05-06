using System.Collections.Generic;
using System.Linq.Expressions;
using Leo.ThirdParty.AutoMapper.Configuration;
using Leo.ThirdParty.AutoMapper.Mappers.Internal;

namespace Leo.ThirdParty.AutoMapper.Mappers
{
    using static CollectionMapperExpressionFactory;

    public class DictionaryMapper : IObjectMapper
    {
        public bool IsMatch(TypePair context) => context.SourceType.IsDictionaryType() && context.DestinationType.IsDictionaryType();

        public Expression MapExpression(IConfigurationProvider configurationProvider, ProfileMap profileMap,
            IMemberMap memberMap, Expression sourceExpression, Expression destExpression, Expression contextExpression)
            => MapCollectionExpression(configurationProvider, profileMap, memberMap, sourceExpression, destExpression, contextExpression, typeof(Dictionary<,>), MapKeyPairValueExpr);
    }
}